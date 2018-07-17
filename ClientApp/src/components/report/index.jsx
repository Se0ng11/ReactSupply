import React, { Component } from 'react';
import Iframe from 'react-iframe';

export class Report extends Component {

    render() {
        const menu = this.props.menu;
        if (menu.length !== 0) {
            localStorage.setItem("reportMenu", JSON.stringify(menu));
        }

        if (this.props.match.params.id !== undefined) {

            let selectedUrl = (JSON.parse(localStorage.getItem("reportMenu")).filter(menu => menu.SubCode === this.props.match.params.id)[0]).Url;
            return (
                <div>
                    <Iframe url={selectedUrl}
                        display="initial"
                        position="relative"
                        styles={{ minHeight: "700px" }}
                        allowFullScreen />
                </div>
            )
        } else {
            return (
                <h2>This is Report Page</h2>    
            )
        }
     
    }

}   