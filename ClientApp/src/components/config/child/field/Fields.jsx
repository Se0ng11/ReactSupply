import React from 'react';
import ReactGrids from '../../../../plugin/dataTable/ReactGrids';

export class Fields extends React.Component {

    getIdentifier = (obj) => {
        return obj.ValueName;
    }

    render =() => {
        return (
            <div>
                <ReactGrids
                    getApi="api/Home/GetConfiguration"
                    postApi="api/Home/PostSingleConfigurationField"
                    identifier={this.getIdentifier}
                    isDoubleHeader={true} />
            </div>
        )
    }

}
