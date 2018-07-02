import React, { Component } from 'react'; 
import { CSVLink } from 'react-csv';
import PropTypes from 'prop-types';
import moment from 'moment';

export class CsvButton extends Component {

    render() {

        let fileName = window.location.pathname.substr(1) + moment().format("YYYYMMDD") + ".csv";
        return (
            <CSVLink data={this.props.body} filename={fileName} className="btn"><i className="fa fa-table" aria-hidden="true"></i> CSV</CSVLink>
        )
    }
}   

CsvButton.propTypes  = {
    header: PropTypes.array,
    body: PropTypes.array
}