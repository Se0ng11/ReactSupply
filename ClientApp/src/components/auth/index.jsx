import '../auth/auth.css';
import React from 'react';
import axios from 'axios';
//import { Link } from "react-router-dom";
import { ToastContainer, toast } from 'react-toastify';

export class Auth extends React.Component {
    constructor() {
        super();

        this.state = {
            userName: "",
            password: "",
            message: ""
        }
        this.onChange = this.onChange.bind(this);
        //if (localStorage.getItem("token") !== null) {
        //    window.location.href = "/home";
            
        //}
    }

    onSubmit= (e) => {
        e.preventDefault();
        axios.post('api/Auth', {
            UserName: this.state.userName,
            Password: this.state.password
        }).then((response) => {
            var data = JSON.parse(response.data);

            if (data.Status === "SUCCESS") {
                var result = JSON.parse(data.Result);
                localStorage.setItem("currentMenu", 0);
                localStorage.setItem("token", result.Token);
                localStorage.setItem("refresh", result.Refresh);
                localStorage.setItem("user", result.UserId);
                window.location.href = "/home";

            } else {
                this.setState({
                    message: data.Result
                });


            }
        }).catch((error) => {
            toast.error(error.message);
        });
    }

    onChange= (e) => {
        this.setState({
            [e.target.name]: e.target.value,
            message: ""
        });
      
    }

    render() {
        return (
            <div>
                <form className="form-signin" onSubmit={this.onSubmit}>
                    <h2 className="form-signin-heading">Sample</h2>
                    <label className="sr-only">Email address</label>
                    <input type="input" name="userName" className="form-control" placeholder="Sample ID" onChange={this.onChange} />
                    <label className="sr-only">Password</label>
                    <input type="password" name="password" className="form-control" placeholder="Password" onChange={this.onChange} />
                    <button type="submit" className="btn btn-lg btn-primary btn-block">Sign In <i className="fa fa-sign-in"></i></button>
                    <span className="error">{this.state.message}</span>
                </form>
                <ToastContainer
                    position="top-left"
                    autoClose={10000}
                    hideProgressBar={false}
                    newestOnTop
                    closeOnClick
                    rtl={false}
                    pauseOnVisibilityChange
                    draggable
                    pauseOnHover
                />
            </div>
        );
    }
}

//<div class="video-background">
//    <div class="video-foreground">
//        <iframe src="https://www.youtube.com/embed/D3FELt4oOk4?controls=0&showinfo=0&rel=0&autoplay=1&loop=1&mute=1&playlist=D3FELt4oOk4" frameborder="0" allowfullscreen></iframe>
//    </div>
//</div>