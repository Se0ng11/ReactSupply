import React from 'react';
import ReactDOM from 'react-dom';
import DatePicker from 'react-datepicker';
import moment from 'moment';
import 'react-datepicker/dist/react-datepicker.css';
import './DatePickerEditor.css';

const { editors: { EditorBase } } = require('react-data-grid');

export class DatePickerEditor extends EditorBase {
    constructor(props) {
        super(props);
        var selectedDate = moment(this.props.value);

        if (!selectedDate.isValid()) {
            selectedDate = moment();
        }

        this.state = {
            startDate: selectedDate,
            value: this.props.rowData[this.props.column.key]
        }

        this.handleSelectChange = this.handleSelectChange.bind(this);
    }

    getInputNode() {
        return ReactDOM.findDOMNode(this);
    }

    getValue = () => {
        return {
            [this.props.column.key]: this.state.value
        };
    }

    onClick() {
        this.getInputNode().focus();
    }

    onDoubleClick() {
        this.getInputNode().focus();
    }

    handleSelectChange = (value) => {
        value = value.format("YYYY/MM/DD");

        this.setState({ value: value }, function () {
            this.props.onCommit();
        });
    }

    render() {
        return (
            <DatePicker
                autoFocus
                showMonthDropdown
                showYearDropdown
                dropdownMode="select"
                todayButton={"Today"}
                maxDate={moment().add(2, 'years')}
                minDate={moment().subtract(2, 'years')}
                selected={this.state.startDate}
                style={this.getStyle()}
                className="form-control"
                onChange={this.handleSelectChange}
                dateFormat="YYYY/MM/DD"
                readOnly={!this.props.column.editable}
            />
        )
    }
}