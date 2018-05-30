import React, { Component } from 'react';
import ReactGrids from '../dataTable/ReactGrids';

export class ReactGridSample extends Component {

    render() {
        return (
            <div>
                <ReactGrids
                    getApi="api/Home/GetSupplyRecord"
                    postApi="api/Home/PostSingleSupplyRecordField"
                    isBasic={false}
                    isDoubleHeader={true} />
            </div>
        );
    }
}
