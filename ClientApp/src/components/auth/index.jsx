import '../auth/auth.css';
import React from 'react';
import axios from 'axios';
import { Redirect } from "react-router-dom";
import { toast } from 'react-toastify';

export class Auth extends React.Component {
    constructor() {
        super();

        this.state = {
            userId: "",
            password: "",
            message: "",
            redirect: false,
            disabled: false,
            appId: "",
            adLink:""
        }
        this.onChange = this.onChange.bind(this);
    }

    componentDidMount() {
        if (localStorage.getItem("token") !== null) {
            this.setState({ redirect: true });
        } else {
            let self = this;

            axios.get('api/Settings/GetAppInfo')
                .then((response) => {
                    let data = JSON.parse(response.data);
                    self.setState({
                        appId: data.appId.Value,
                        adLink: data.adAuth.Value
                    });

            }).catch((error) => {
                self.onError(error.message);
            });
        }
    }

    onSubmit= (e) => {
        e.preventDefault();
        let self = this;
        self.setState({ disabled: true });

        self.onADLogin(self, self.state.userId, "faketoken");

        //axios.post(self.state.adLink, {
        //    AppId: self.state.appId,
        //    UserId: self.state.userId,
        //    Password: self.state.password
        //}).then((response) => {
        //    var data = response.data.Data;
        //    if (data !== null) {
        //        self.onADLogin(self, self.state.userId, data.Token);
        //    } else {
        //        self.setState({
        //            message: response.data.ResponseMessage,
        //            disabled: false
        //        });
        //    }

        //}).catch((error) => {
        //    self.onError(error.message);
        //});
    }

    onADLogin=(self, userId, adToken) => {
        axios.post('api/Auth/LoginAsync', {
             UserName: userId
        }).then((response) => {
            var data = JSON.parse(response.data);

            if (data.Status === "SUCCESS") {

                var result = JSON.parse(data.Result);
                localStorage.setItem("currentMenu", 0);
                localStorage.setItem("token", result.Token);
                localStorage.setItem("refresh", result.Refresh);
                localStorage.setItem("user", result.UserId);
                localStorage.setItem("role", result.Role);
                localStorage.setItem("adToken", adToken);
                self.setState({redirect: true, disabled: false});
            } else {
                self.setState({
                    message: data.Result,
                    disabled: false
                });
            }
        }).catch((error) => {
            self.onError(error.message);
        });
    }

    onError = (error) => {
        this.setState({
            disabled: false
        });
        toast.error(error);
    }
    onChange= (e) => {
        this.setState({
            [e.target.name]: e.target.value,
            message: ""
        });
    }

    render() {
       
        let redirect = this.state.redirect;
       
        if (redirect) {
            return <Redirect to="/home" />
        }
        return (
            <div>
                <form className="form-signin" onSubmit={this.onSubmit}>
                    <h2 className="form-signin-heading">Supply Chain Tracker</h2>
                    <label className="sr-only">Email address</label>
                    <input type="input" name="userId" className="form-control" placeholder="ID" onChange={this.onChange} disabled={this.state.disabled} />
                    <label className="sr-only">Password</label>
                    <input type="password" name="password" className="form-control" placeholder="Password" onChange={this.onChange} disabled={this.state.disabled} />
                    <button type="submit" className="btn btn-lg btn-primary btn-block" disabled={this.state.disabled}>Sign In <i className="fa fa-sign-in"></i></button>
                    <span hidden={!this.state.disabled}><i className="fa fa-spinner fa-spin fa-2x"></i> Verify access info...</span>
                    <span className="error" hidden={this.state.disabled}>{this.state.message}</span>
                </form>

            </div>
        );
    }
}

//<div class="video-background">
//    <div class="video-foreground">
//        <iframe src="https://www.youtube.com/embed/D3FELt4oOk4?controls=0&showinfo=0&rel=0&autoplay=1&loop=1&mute=1&playlist=D3FELt4oOk4" frameborder="0" allowfullscreen></iframe>
//    </div>
//</div>
