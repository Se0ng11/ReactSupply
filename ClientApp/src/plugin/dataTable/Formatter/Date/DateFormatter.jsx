import React from 'react';

export class DateFormatter extends React.Component {


    render() {
        let self = this;

        return (
            <div className="due-date">
                <span>{self.props.value}</span>
            </div>
        );
    }
}