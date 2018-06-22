import React, { Component } from 'react';
import axios from 'axios';
import { Top, Burger } from '../plugin/nav/NavList';
import { Col, Grid, Row } from 'react-bootstrap';
import { ToastContainer, toast } from 'react-toastify';

export class Layout extends Component {
    constructor(props) {
        super(props);

        this.state = {
            top: [],
            left: []
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

    render() {
        let currentMenu = this.state.left.filter(menu => menu.MenuCode === localStorage.getItem("currentMenu"));

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
