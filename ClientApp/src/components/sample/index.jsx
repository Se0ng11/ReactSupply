import React, { Component } from 'react';
import ReactGrids from '../../plugin/dataTable/ReactGrids';

export class Sample extends Component {

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
