import React, { Component } from 'react'; 
import ReactGrids from '../../plugin/dataTable/ReactGrids';
import { Users } from './child';

export class Config extends Component {
    constructor(props) {
        super(props);
    }


    //componentWillReceiveProps(nextProps) {
    //    let something = nextProps;
    //}

    render() {
        let page = this.props.match.params.id;
        //4001
        //4002
        //4003
        //4004
        if (page === "4001") {
            return (
                <Users />    
            )
        } else if (page === "4003") {
            return (
                <div>
                    <ReactGrids
                        getApi="api/Home/GetConfiguration"
                        postApi="api/Home/PostSingleConfigurationField"
                        isBasic={true}
                        isDoubleHeader={true} />
                </div>
            );
        } else if (page === "4001") {

        } else if (page === "4004") {

        }

        return (
            <h2>This is config page</h2>
        );
       
    }
}