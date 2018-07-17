import './container.css';
import React from 'react';

export default class Container extends React.Component {

    render() {
        return (
            <div className="container-border">
                {this.props.children}
            </div>    
        );
    }
}