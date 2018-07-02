import React from 'react';
import PropTypes from 'prop-types';

export class AddButton extends React.Component {

    render() {
        return (
            <div className="grid-btn-left">
                <button type="button" className="btn" onClick={this.props.onClick}> <i className="fa fa-plus"></i> Add</button>
            </div>
        )
    }
}

AddButton.propTypes = {
    isSow: PropTypes.bool
}