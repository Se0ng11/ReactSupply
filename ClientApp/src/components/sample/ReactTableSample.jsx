import React, { Component } from 'react';
import ReactTables from '../dataTable/ReactTables';
import { Home } from '../Home';

export class ReactTableSample extends Component {

    render() {
        return (
            <Home>
                <ReactTables api="api/Home/GetConfigurationMainJson" />
            </Home>
        );
    }
}