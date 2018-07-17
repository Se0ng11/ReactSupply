import React, { Component } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import Card from '../../plugin//card/Card';
import Container from '../../plugin/container/Container';
import { Grid, Row, Col } from 'react-bootstrap';
import LineChart from '../../plugin/chart/LineChart';
import HorizontalChart from '../../plugin/chart/HorizontalChart';

export class Home extends Component {

    componentDidMount() {
        //load data here and do something
        let self = this;

        self.onLoad();
    }

    onLoad = () => {
        //axios.get(this.props.getApi,
        //    {
        //        params: {
        //            identifier: localStorage.getItem("currentMenu")
        //        },
        //        cancelToken: new CancelToken(function executor(c) {
        //            cancel = c;
        //        })
        //    }).then((response) => {
                

        //    })
        //    .catch((error) => {

        //        if (error.response !== undefined) {
        //            let msg = error.message + ": " + error.response.statusText;
        //            toast.error(msg);
        //        }
        //    });

    }


    render() {
        return (
            <Grid fluid>
                <Row>
                    <Col xs={6} md={3}>
                        <Card name="OTD %" value="70" color="green" icon="industry" />
                    </Col>
                    <Col xs={6} md={3}>
                        <Card name="Inspection" value="31.9" color="red" icon="search" />
                    </Col>
                    <Col xs={6} md={3}>
                        <Card name="HotBox" value="19" color="yellow" icon="cube" />
                    </Col>
                    <Col xs={6} md={3}>
                        <Card name="Pm Delay case" value="10" color="red" icon="clock-o" />
                    </Col>
                </Row>

                <Row>
                    <Col xs="12" md="6">
                        <Container>
                            <LineChart />
                        </Container>
                    </Col>
                    <Col xs="12" md="6">
                        <Container>
                            <HorizontalChart />
                        </Container>
                    </Col>
                </Row>
            </Grid>
        );
    }
}
