import React, { Component } from 'react';
import ReactTable from "react-table";
import 'react-table/react-table.css';

export default class ReactTables extends Component {

    constructor(props) {
        super(props);
        this.state = {
            data: null
        };

        this.renderEditable = this.renderEditable.bind(this);
    }

    componentDidMount() {
        fetch(this.props.api)
            .then(response => response.json())
            .then(data => {
                this.setState({ _columns: JSON.parse(data) });
            });
    }

    renderEditable(cellInfo) {
        return (
            <div className="form-control"
                style={{ backgroundColor: "#fafafa" }}
                contentEditable
                suppressContentEditableWarning
                onBlur={e => {
                    const data = [...this.state.data];
                    data[cellInfo.index][cellInfo.column.id] = e.target.innerHTML;
                    this.setState({ data });
                }}
                dangerouslySetInnerHTML={{
                    __html: this.state.data[cellInfo.index][cellInfo.column.id]
                }}
            />
        );
    }

    render() {
        return (
            <div>
                <ReactTable
                    columns={[
                        {
                            Header: "Name",
                            columns: [
                                {
                                    Header: "First Name",
                                    accessor: "firstName"
                                },
                                {
                                    Header: "Last Name",
                                    id: "lastName",
                                    accessor: d => d.lastName
                                }
                            ]
                        },
                        {
                            Header: "Info",
                            columns: [
                                {
                                    Header: "Age",
                                    accessor: "age"
                                },
                                {
                                    Header: "Status",
                                    accessor: "status"
                                }
                            ]
                        },
                        {
                            Header: 'Stats',
                            columns: [
                                {
                                    Header: "Visits",
                                    accessor: "visits"
                                }
                            ]
                        }
                    ]}
                    defaultPageSize={100}
                    showPageSizeOptions={false}
                    className="-striped -highlight"
                />
                <br />
            </div>
        );
    }
}