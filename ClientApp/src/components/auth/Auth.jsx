import React, { Component } from 'react';
import '../auth/auth.css';
import { Link } from "react-router-dom";

export default class Auth extends Component {
    render() {
        return (
            <div>
                <form className="form-signin">
                    <h2 className="form-signin-heading">Supply Chain</h2>
                    <label className="sr-only">Email address</label>
                    <input type="email" id="inputEmail" className="form-control" placeholder="Hartalega ID" />
                    <label className="sr-only">Password</label>
                    <input type="password" id="inputPassword" className="form-control" placeholder="Password" />
                    <Link className="btn btn-lg btn-primary btn-block" to='/home' >Sign In <i className="fa fa-sign-in"></i></Link>
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