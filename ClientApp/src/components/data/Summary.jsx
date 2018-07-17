import React, { Component } from 'react';
import ReactGrids from '../../plugin/dataTable/ReactGrids';

export class Summary extends Component {

    render() {
        return (
            <div>
                <ReactGrids
                    headerRowHeight={90}
                    getApi="api/Home/GetSupplyRecord"
                    postApi="api/Home/PostSupplyRecords"
                    isGroupButton={false}
                    isHistoryEnabled={false}
                />
            </div>
        );
    }
}
