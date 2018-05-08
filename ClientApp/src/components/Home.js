import React, { Component } from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import Top from '../components/menu/Top';

export class Home extends Component {
    render() {
        return (
            <div>
                <Top />
                <Grid fluid>
                    <Row>
                        <Col sm={12}>
                            {this.props.children}
                        </Col>
                    </Row>
                </Grid>
            </div>
        );
    }
}