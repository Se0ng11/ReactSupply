import React, { Component } from 'react'; 
import { Users, Fields } from './child';

export class Config extends Component {

    //componentWillReceiveProps(nextProps) {
    //    let something = nextProps;
    //}

    render() {
        let page = this.props.match.params.id;
        //4001 ,lu

        //4002
        //4003
        //4004
        if (page === "6001") {
            return (
                <Users />    
            )
        } else if (page === "6002") {
            return (
                <Fields />
            );
        } else if (page === "4003") {

        } else if (page === "4004") {

        }

        return (
            <h2>This is Config Page</h2>
        );
       
    }
}