import React, { Component } from 'react';
import DatePicker from 'react-datepicker';
import moment from 'moment';
import ReactGrids from '../../plugin/dataTable/ReactGrids';
import { Grid, Row, Col } from 'react-bootstrap';

export class Details extends Component {
    constructor(props) {
        super(props);

        this.state = {
            parameters: ""
        }
    }
    
    getIdentifier = (obj, keys) => {
        if (keys === 1) {
            return obj.Identifier;
        } else if (keys === 2) {
            return obj.SizeRatio;
        }
    }

    onSearchClick = (param) => {
        let self = this;
        const obj = {
            startDate: param.startDate.format("YYYY/MM/DD"),
            endDate: param.endDate.format("YYYY/MM/DD"),
            status: param.status
        }
        self.setState({
            parameters: JSON.stringify(obj)
        });
    }

    render() {
        return (
            <div>
                <ReactGrids
                    headerRowHeight={80}
                    getApi="api/Home/GetSupplyRecord"
                    postApi="api/Home/PostSupplyRecords"
                    identifier={this.getIdentifier}
                    refreshGrid={true}
                    isGroupButton={true}
                    isHistoryEnabled={true}
                    search={<SearchCriteria search={this.onSearchClick} />}
                    parameters={this.state.parameters}
                />
            </div>
        );
       
    }
}

class SearchCriteria extends React.Component {
    constructor(props) {
        super(props);

        let startDate = moment().startOf('month').format('YYYY/MM/DD');
        let endDate = moment().endOf('month').format('YYYY/MM/DD');

        this.state = {
            startDate: moment(startDate),
            endDate: moment(endDate),
            status: ""
        }
    }

    onIssuedFrom = (e) => {
        this.setState({
            startDate: e
        });
    }

    onIssuedTo = (e) => {
        this.setState({
            endDate: e
        });
    }

    onChange = (e) => {
        this.setState({
            [e.target.name]: e.target.value,
        });
    }

    onSearchClick = () => {
        let self = this;
        self.props.search(self.state);
    }

    render() {
        let self = this;

        return (
            <Grid fluid>
                <Row>
                    <Col xs={12} md={2}>
                        <label>Issued Date From</label>
                    </Col>
                    <Col xs={12} md={2}>
                        <DatePicker
                            name="startDate"
                            showMonthDropdown
                            showYearDropdown
                            //isClearable={true}
                            dropdownMode="select"
                            todayButton={"Today"}
                            className="form-control"
                            dateFormat="YYYY/MM/DD"
                            selected={self.state.startDate}
                            onChange={self.onIssuedFrom}
                            readOnly={true}
                        />
                    </Col>
                    <Col xs={12} md={2}>
                        <label>Issued Date To</label>
                    </Col>
                    <Col xs={12} md={2}>
                        <DatePicker
                            name="endDate"
                            showMonthDropdown
                            showYearDropdown
                            //isClearable={true}
                            dropdownMode="select"
                            todayButton={"Today"}
                            className="form-control"
                            dateFormat="YYYY/MM/DD"
                            selected={self.state.endDate}
                            onChange={self.onIssuedTo}
                            readOnly={true}
                        />
                    </Col>
                    <Col xs={12} md={2}>
                        <label>Container Truck Out Status</label>
                    </Col>
                    <Col xs={12} md={2}>
                        <select name="status" className="form-control" value={self.state.status} onChange={self.onChange}>
                            <option value=""></option>
                            <option value="1">On Time</option>
                            <option value="0">Delay</option>
                        </select>
                    </Col>
                </Row>
                <hr />
                <Row>
                    <button type="button" className="btn btn-default pull-right" onClick={this.onSearchClick}><i className="fa fa-search"></i> Search</button>
                </Row>
            </Grid>
        );
    }
}
