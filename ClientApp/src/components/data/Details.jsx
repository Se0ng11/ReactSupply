import React, { Component } from 'react';
import ReactGrids from '../../plugin/dataTable/ReactGrids';

export class Details extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isInitialLoad: true
        }
    }

    componentDidMount = () => {
        this.setState({
            isInitialLoad: false
        })
    }

    componentWillReceiveProps = () => {
    }

    getIdentifier = (obj, keys) => {
        if (keys === 1) {
            return obj.Identifier;
        } else if (keys === 2) {
            return obj.SizeRatio;
        }
    }

    render() {
        if (!this.state.isInitialLoad) {
            return (
                <div>
                    <ReactGrids
                        headerRowHeight={80}
                        getApi="api/Home/GetSupplyRecord"
                        postApi="api/Home/PostSupplyRecords"
                        identifier={this.getIdentifier}
                        isGroupButton={true}
                        refreshGrid={!this.state.isInitialLoad}
                        isHistoryEnabled={true}
                    />
                </div>
            );
        }

        return (
            <div>

            </div>
        );
       
    }
}
