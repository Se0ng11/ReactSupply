import React, { Component } from 'react';
import ReactGrids from '../../plugin/dataTable/ReactGrids';

export class Sample extends Component {

    getIdentifier = (obj, keys) => {
        if (keys === 1) {
            return obj.Identifier;
        } else if (keys === 2) {
            return obj.SizeRatio;
        }
    }

    render() {
        return (
            <div>
                <ReactGrids
                    getApi="api/Home/GetSupplyRecord"
                    postApi="api/Home/PostSupplyRecords"
                    identifier={this.getIdentifier}
                    isDoubleHeader={true}
                    isGroupButton={true}
                />
            </div>
        );
    }
}
