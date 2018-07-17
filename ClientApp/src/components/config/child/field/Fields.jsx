import React from 'react';
import ReactGrids from '../../../../plugin/dataTable/ReactGrids';

export class Fields extends React.Component {

    getIdentifier = (obj, keys) => {
        if (keys === 1) {
            return obj.ModuleId;
        } else if (keys === 2) {
            return obj.ValueName;
        }
    }

    render =() => {
        return (
            <div>
                <ReactGrids
                    headerRowHeight={50}
                    getApi="api/Home/GetConfiguration"
                    postApi="api/Home/PostSingleConfigurationField"
                    identifier={this.getIdentifier}
                    isHistoryEnabled={true}
                />
            </div>
        )
    }

}
