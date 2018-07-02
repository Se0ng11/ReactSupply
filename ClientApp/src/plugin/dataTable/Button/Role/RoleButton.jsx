import React from 'react';
import PropTypes from 'prop-types';

export class RoleButton extends React.Component {

    render() {
        return (
            <div className="grid-btn-left">
                <button type="button" className="btn" onClick={this.props.onClick}> <i className="fa fa-id-badge"></i> Role</button>
            </div>
        )
    }
}

RoleButton.propTypes = {
    isSow: PropTypes.bool
}