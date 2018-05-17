import React, { Component } from 'react';
import ReactDataGrid from 'react-data-grid';
import axios from 'axios';
import update from 'immutability-helper';
import ErrorBoundary from '../error/ErrorBoundary';
import PropTypes from 'prop-types';
import { DatePickerEditor } from './Editors/DatePickerEditor';
import { Csv } from './Exporter/Csv';
import '../dataTable/ReactGrids.css';

const { Toolbar, Editors,  Filters: { NumericFilter, AutoCompleteFilter, MultiSelectFilter, SingleSelectFilter, DateFilter }, Data: { Selectors } } = require('react-data-grid-addons');
//Formatters,
const { DropDownEditor } = Editors;
//AutoComplete: AutoCompleteEditor, DateRangeEditor
export default class ReactGrids extends Component {
    displayName = ReactGrids.name

    constructor(props, context) {
        super(props, context);

        this.state = {
            header: [],
            rows: [],   
            filters: {},
            sortColumn: null,
            sortDirection: null,
            cellUpdateCss: "",
            cellKey: null,
            cellIdx: null
        };
    }

    componentDidMount() {
        var self = this;
        axios.get(this.props.getApi)
            .then((response) => {
                var data = response.data;
                var obj = JSON.parse(data);
                var _header = JSON.parse(obj.header);
                var _rows = JSON.parse(obj.body);

                for (var i = 0; i <= _rows.length - 1; i++) {
                    _rows[i].No = i + 1;
                }

                self.setState({ header: _header, rows: _rows });
            })
            .catch((error) => {
                console.log(error);
            });
    }

    rowGetter = (rowIdx) => {
        let rows = this.getRows();

        return rows[rowIdx];
    };

    handleGridRowsUpdated = ({ cellKey, fromRow, toRow, updated, action, originRow }) => {
        let rows = Selectors.getRows(this.state);
        var self = this;

        self.setState({ cellUpdateCss: "border-progress" });
        for (let i = fromRow; i <= toRow; i++) {
            let rowToUpdate = rows[i];
            let updatedRow = update(rowToUpdate, { $merge: updated });
  
            if (JSON.stringify(rowToUpdate) !== JSON.stringify(updatedRow))
            {
                rows[i] = updatedRow;

                this.setState({
                    cellKey: cellKey,
                    cellIdx: i
                })

                if (this.props.isBasic)
                {

                }
                else
                {
                    this.postToServer(updatedRow.AX6SO, JSON.stringify(updated));
                }
            }
        }

        this.setState({ rows });
    };

    postToServer = (aX, vC) => {
        var self = this;
        axios.put(this.props.postApi, {
            identifier: aX,
            updated: vC
        })
        .then((response) => {
            self.setState({ cellUpdateCss: "border-success" });
        })
        .catch((error) => {
            console.log(error);
            self.setState({ cellUpdateCss: "border-failed" });
        });
    }

    handleGridSort = (sortColumn, sortDirection) => {
        this.setState({ sortColumn: sortColumn, sortDirection: sortDirection });
    };

    handleFilterChange = (filter) => {
        let newFilters = Object.assign({}, this.state.filters);
        if (filter.filterTerm) {
            newFilters[filter.column.key] = filter;
        } else {
            delete newFilters[filter.column.key];
        }
        this.setState({ filters: newFilters });
    };

    onClearFilters = () => {
        this.setState({ filters: {} });
    };

    getRows = () => {
        return Selectors.getRows(this.state);
    };

    getSize = () => {
        return this.getRows().length;
    };

    getValidFilterValues = (columnId) => {
        let values = this.state.rows.map(r => r[columnId]);
        return values.filter((item, i, a) => { return i === a.indexOf(item); });
    };

    renderStringToObject = (apiData) => {
        for (var i = 0; i <= apiData.length - 1; i++) {
            var s = apiData[i];

            s.headerRenderer = this.renderStringToHeaderRenderer(s);
            s.editor = this.renderControlToEditor(s);

            if (s.filterRenderer !== undefined)
                s.filterRenderer = this.renderStringToFilter(s);

            if (s.formatter !== undefined)
                s.formatter = this.renderStringToFormatter(s);

            if (s.editor !== undefined)
                s.editor = this.renderStringToEditor(s);
        }
        return apiData;
    }

    renderStringToFilter = (ary) => {
        ary = ary.filterRenderer;
        if (ary === "MultiSelectFilter") {
            ary = MultiSelectFilter;
        }
        else if (ary === "NumericFilter") {
            ary = NumericFilter;
        }
        else if (ary === "AutoCompleteFilter") {
            ary = AutoCompleteFilter;
        }
        else if (ary === "SingleSelectFilter") {
            ary = SingleSelectFilter;
        }
        else if (ary === "DateFilter") {
            ary = DateFilter;
        }

        return ary;
    };

    renderStringToEditor = (ary) => {
        if (ary.editor === "DropDown") {
            ary.editor = <DropDownEditor />;
        }

        return ary.editor;
    }

    renderControlToEditor = (ary) => {
        if (ary.control === "date") {
            ary.editor = <DatePickerEditor />;
        }

        return ary.editor;
    }

    renderStringToHeaderRenderer = (ary) => {

        if (ary.headerRenderer === undefined) {
            if (ary.Group === undefined)
                ary.Group = "";

            ary.headerRenderer = <HeaderGroup parent={ary.title} child={ary.name} />;
        }

        return ary.headerRenderer;
    }

    renderStringToFormatter = (ary) => {
        return ary.formatter;
    }

    render() {
        let apiData = this.state.header;

        if (apiData !== undefined) {
            if (!this.props.isBasic) {
                apiData = this.renderStringToObject(apiData);
                return (
                    <div>
                        <ErrorBoundary>
                            <ReactDataGrid
                                onGridSort={this.handleGridSort}
                                enableCellSelect={true}
                                columns={apiData}
                                rowGetter={this.rowGetter}
                                rowsCount={this.getSize()}
                                headerRowHeight={100}
                                minHeight={650}
                                cellNavigationMode="changeRow"
                                onGridRowsUpdated={this.handleGridRowsUpdated}
                                toolbar={<Toolbar enableFilter={true} children={<Csv header={this.state.header} body={this.state.rows} />} />}
                                onAddFilter={this.handleFilterChange}
                                getValidFilterValues={this.getValidFilterValues}
                                onClearFilters={this.onClearFilters}
                                emptyRowsView={EmptyRowsView}
                            />
                        </ErrorBoundary>
                    </div>
                )
            }
            else
            {

            }


        }
        return (<div>Loading....</div>);

    }
}

ReactGrids.propTypes = {
    getApi: PropTypes.string,
    postApi: PropTypes.string,
    isBasic: PropTypes.bool
}

class EmptyRowsView extends React.Component {
    render() {
        return (<div>Nothing to show</div>);
    }
}

class HeaderGroup extends React.Component {
    render() {
        if (this.props.parent !== undefined) {
            return (
                <div>
                    <div>{this.props.parent}</div>
                    <hr />
                    <div>{this.props.child}</div>
                </div>
            )
        } else {
            return (
                <div>
                    <div style={ divStyle }>{this.props.child}</div>
                </div>
            )
        }
    }
}

const divStyle = {
    height: '90px'
};
