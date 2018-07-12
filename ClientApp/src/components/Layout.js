import React, { Component } from 'react';
import axios from 'axios';
import { Top, Burger } from '../plugin/nav/NavList';
import { Col, Grid, Row } from 'react-bootstrap';
import { toast } from 'react-toastify';
import { Redirect } from 'react-router-dom';
//import IdleTimer from 'react-idle-timer';

export class Layout extends Component {
    constructor(props) {
        super(props);

        this.state = {
            top: [],
            left: [],
            redirect: false,
            //timeOut: 900000
        }
    }

    componentDidMount() {
        const self = this;
        axios.get("api/Menu/GetMenu",
        ).then((response) => {
                const data = JSON.parse(response.data);
                self.setState({ top: JSON.parse(data.top), left: JSON.parse(data.left) });
        })
        .catch((error) => {
            let msg = "Layout() " + error.message + ": " + error.response.statusText;
            toast.error(msg);
        });
    }

    _onIdle = () => {
        localStorage.clear();
        this.setState({ redirect: true });
    }

    _onActive = () => {
     
    }

    render() {
        let currentMenu = this.state.left.filter(menu => menu.MenuCode === localStorage.getItem("currentMenu"));
        let isRedirect = this.state.redirect;

        if (isRedirect) {
            return <Redirect to="/" />
        }

        return (
            <div>
                <Top menu={this.state.top} />
                {
                    currentMenu.length > 0 &&
                    <Burger {...this.props} menu={this.state.left} />
                }
    
                <Grid fluid id="page-wrap">
                    <Row>
                        <Col sm={12}>
                            {React.cloneElement(this.props.children, { menu: currentMenu })}
                        </Col>
                    </Row>
                </Grid>
            </div>
        );
    }
}
//<IdleTimer
//    ref="idleTimer"
//    element={document}
//    activeAction={this._onActive}
//    idleAction={this._onIdle}
//    timeout={this.state.timeOut}>
//    <Top menu={this.state.top} />
//    {
//        currentMenu.length > 0 &&
//        <Burger {...this.props} menu={this.state.left} />
//    }

//    <Grid fluid id="page-wrap">
//        <Row>
//            <Col sm={12}>
//                {React.cloneElement(this.props.children, { menu: currentMenu })}
//            </Col>
//        </Row>
//    </Grid>
//</IdleTimer>