import '../notFound/notfound.css';
import React, { Component } from 'react';

export class NotFound extends Component {

    render() {
        return (
            <div>
                <div className="warning">
                    <h1>404 Page not found</h1>
                    <h3>You might not authorize to access this page or the page has been remove</h3>
                    <h3>Please contact admin for more info</h3>
                </div>
            </div>
        )

    }

}