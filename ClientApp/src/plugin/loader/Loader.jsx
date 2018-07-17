import './loader.css';
import React from 'react';
import PropTypes from 'prop-types';

export default class Loader extends React.Component {

    render() {
        let self = this;
        if (self.props.show) {
            return (
                <div>
                    <div className="loader">
                        <div className="spinner">
                            <i className="fa fa-spinner fa-spin fa-2x"></i> Loading...
                        </div>
                    </div>

                  
                   
                </div>
            );
        }

        return (<div></div>);
    
    }

}

Loader.propTypes = {
    show: PropTypes.bool
}


