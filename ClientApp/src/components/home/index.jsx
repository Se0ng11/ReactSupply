import React, { Component } from 'react';

export class Home extends Component {

    render() {
        let user = localStorage.getItem("user");
        let role = localStorage.getItem("role");
        return (
            <div>
                <h2>Welcome back {user}</h2>
                <h4>Current role is {role}</h4>
                <h4>You had xxx record(s) waiting to work on</h4>
            </div>
        );
    }
}