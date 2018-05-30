import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import { Top, Burger } from '../components/menu/MenuList';
export class Layout extends Component {

    render() {
        const isAuth = (this.props.location === undefined || this.props.location === "/" ? false : true);

        if (isAuth) {
            return (
                <div>
                    <Top />
                    <Burger />
                    <Grid fluid id="page-wrap">
                        <Row>
                            <Col sm={12}>
                                {this.props.children}
                            </Col>
                        </Row>
                    </Grid>
                </div>
            );
        } else {
            return (
                <div>
                    {this.props.children}
                </div>    
            )
        }
       
    }
}