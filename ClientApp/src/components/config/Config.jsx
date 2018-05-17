import React, { Component } from 'react'; 
import ReactGrids from '../dataTable/ReactGrids';
import { Home } from '../Home';

export class Config extends Component {

    render() {
        return (
            <Home>
                <ReactGrids
                    getApi="api/Home/GetConfiguration"
                    postApi="api/Home/PostSingleConfigurationField"
                    isBasic={true} />
            </Home>
        );
    }
}