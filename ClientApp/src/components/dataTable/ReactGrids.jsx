import React, { Component } from 'react';
import ReactDataGrid from 'react-data-grid';
import update from 'immutability-helper';
import ErrorBoundary from '../error/ErrorBoundary';
const { Toolbar, Editors, Formatters, Filters: { NumericFilter, AutoCompleteFilter, MultiSelectFilter, SingleSelectFilter, DateFilter }, Data: { Selectors } } = require('react-data-grid-addons');

export default class ReactGrids extends Component {
    displayName = ReactGrids.name

    constructor(props, context) {
        super(props, context);

        let originalRows = this.createRows(1000);
        this.state = { rows: this.createRows(1000), filters: {}, sortColumn: null, sortDirection: null };
    }

    componentDidMount() {
        fetch(this.props.api)
            .then(response => response.json())
            .then(data => {
                this.setState({ _columns: JSON.parse(data) });
            });
    }

    getRandomDate = (start, end) => {
        return new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime())).toLocaleDateString();
    };

    createRows = (numberOfRows) => {
        let rows = [];
        for (let i = 1; i < numberOfRows; i++) {
            rows.push({
                No: i,
                PO: 'Task ' + i,
                AX6SO: Math.min(100, Math.round(Math.random() * 110)),
                priority: ['Critical', 'High', 'Medium', 'Low'][Math.floor((Math.random() * 3) + 1)],
                ProductType: ['Bug', 'Improvement', 'Epic', 'Story'][Math.floor((Math.random() * 3) + 1)],
                SoReceivedDate: this.getRandomDate(new Date(2015, 3, 1), new Date()),
                SoIssuedDate: this.getRandomDate(new Date(), new Date(2016, 0, 1))
            });
        }
        return rows;
    };

    rowGetter = (rowIdx) => {
        let rows = this.getRows();
        return rows[rowIdx];
    };


    handleGridRowsUpdated = ({ fromRow, toRow, updated }) => {
        let rows = this.state.rows.slice();

        for (let i = fromRow; i <= toRow; i++) {
            let rowToUpdate = rows[i];
            let updatedRow = update(rowToUpdate, { $merge: updated });
            rows[i] = updatedRow;
        }

        this.setState({ rows });
    };

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

    renderStringToFilter = (apiData) => {
        for (var i = 0; i <= apiData.length - 1; i++) {
            var s = apiData[i];

            if (s.filterRenderer !== undefined) {
                if (s.filterRenderer === "MultiSelectFilter") {
                    s.filterRenderer = MultiSelectFilter;
                }
                else if (s.filterRenderer === "NumericFilter") {
                    s.filterRenderer = NumericFilter;
                }
                else if (s.filterRenderer === "AutoCompleteFilter") {
                    s.filterRenderer = AutoCompleteFilter;
                }
                else if (s.filterRenderer === "SingleSelectFilter") {
                    s.filterRenderer = SingleSelectFilter;
                }
                else if (s.filterRenderer === "DateFilter") {
                    s.filterRenderer = DateFilter;
                }
            }
        }

        return apiData;
    };

    renderStringToEditor = (apiData) => {


    }

    render() {
        let apiData = this.state._columns;
        let total
        if (apiData !== undefined) {
            apiData = this.renderStringToFilter(apiData);
            return (
                <div>
                    <ErrorBoundary>
                        <ReactDataGrid
                            onGridSort={this.handleGridSort}
                            enableCellSelect={true}
                            columns={apiData}
                            rowGetter={this.rowGetter}
                            rowsCount={this.getSize()}
                            minHeight={600}
                            cellNavigationMode="changeRow"
                            onGridRowsUpdated={this.handleGridRowsUpdated}
                            toolbar={<Toolbar enableFilter={true} />}
                            onAddFilter={this.handleFilterChange}
                            getValidFilterValues={this.getValidFilterValues}
                            onClearFilters={this.onClearFilters}
                            emptyRowsView={EmptyRowsView}/>
                    </ErrorBoundary>
                </div>  
            )
        }
        return (<div>Loading....</div>);

    }
}

class EmptyRowsView extends React.Component {
    render() {
        return (<div>Nothing to show</div>);
    }
}