import React, { Component } from 'react';
import ReactGrids from '../dataTable/ReactGrids';
import { Home } from '../Home';

export class ReactGridSample extends Component {

    render() {
        return (
            <Home>
                <ReactGrids
                    getApi="api/Home/GetSupplyRecord"
                    postApi="api/Home/PostSingleSupplyRecordField"
                    isBasic={false} />
            </Home>
        );
    }
}
