import React from 'react';

export class BooleanFormatter extends React.Component {
    render() {
        let trueFalse = this.props.value.toString();
        return (
            <div>
                <span className={trueFalse}>{trueFalse}</span>
            </div>
        );
    }
}