import React, { Component } from 'react'; 
import ReactGrids from '../../plugin/dataTable/ReactGrids';

export class Config extends Component {

    render() {
        return (
            <div>
                <ReactGrids
                    getApi="api/Home/GetConfiguration"
                    postApi="api/Home/PostSingleConfigurationField"
                    isBasic={true}
                    isDoubleHeader={false} />
            </div>
        );
    }
}