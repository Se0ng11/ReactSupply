import '../dataTable/ReactGrids.css';
import React, { Component } from 'react';
import ReactDataGrid, { Row, Cell } from 'react-data-grid';
import axios from 'axios';
import update from 'immutability-helper';
import PropTypes from 'prop-types';
import { DatePickerEditor } from './Editors';
import { BooleanFormatter, EmptyRowFormatter, HistoryModal, GroupModal } from './Formatter';
import { CsvButton } from './Button';
import { toast } from 'react-toastify';
import moment from 'moment';

const { Toolbar, Editors,  Filters: { NumericFilter, AutoCompleteFilter, MultiSelectFilter, SingleSelectFilter, DateFilter }, Data: { Selectors } } = require('react-data-grid-addons');
//Formatters,
const { DropDownEditor } = Editors;
//AutoComplete: AutoCompleteEditor, DateRangeEditor

const BoolItem = [{ id: 'false', title: 'false', value: 'false' }, { id: 'true', title: 'true', value: 'true' }];
const ControlItem = [{ id: 'boolean', title: 'boolean', value: 'boolean' }, { id: 'date', title: 'date', value: 'date' }, { id: 'input', title: 'input', value: 'input' }]; //{ id: 'dropdown', title: 'dropdown', value: 'dropdown' },

const RowContext = React.createContext('Row');
const ColContext = React.createContext('Col');
const ColorContext = React.createContext('Color');

const CancelToken = axios.CancelToken;
let cancel;

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
            cellCss: "",
            cellRow: null,
            cellCol: null,
            height: window.screen.availHeight - 250,
            isHistoryModal: false,
            isGroupModal: false,
            isGroupClicked: false,
            historyData: {
                header: [],
                body:[]
            },
            groupData: {
                header: [],
                body: []
            }
        };

        //this.onClickShowModal = this.onClickShowModal.bind(this);
    }

    componentDidMount() {
        this.onLoad();
    }

    componentWillReceiveProps = (props) => {

        if (this.props.refreshGrid) {
            this.onLoad();
        }
    }

    componentWillUnmount() {
        cancel("User cancel it");
    }

    rowGetter = (rowIdx) => {
        let rows = this.getRows();

        return rows[rowIdx];
    };

    handleGridRowsUpdated = ({ cellKey, fromRow, toRow, updated, action, originRow }) => {
        var self = this;
        let rows = Selectors.getRows(self.state);

        if (self.state.cellCss === "border-success") {
            self.setState({ cellCss: "" });
        }
       
        for (let i = fromRow; i <= toRow; i++) {
            let rowToUpdate = rows[i];
            let updatedRow = update(rowToUpdate, { $merge: updated });
            let oldData = JSON.stringify(rowToUpdate);
            let newData = JSON.stringify(updatedRow);

            if (oldData !== newData)
            {
                self.setState({ cellCss: "border-progress" });
                rows[i] = updatedRow;

                self.setState({
                    cellRow: newData,
                    cellCol: cellKey
                })

                let id1 = self.props.identifier(updatedRow, 1);
                let id2 = self.props.identifier(updatedRow, 2);
                self.postToServer(id1, id2, JSON.stringify(updated));
            }
        }
        self.setState({ rows });
    };

    postToServer = (id1, id2, updatedVal) => {
        const self = this;
        if (self.props.postApi !== "") {
            axios.post(self.props.postApi,
                {
                    identifier: id1,
                    identifier1: id2,
                    updated: updatedVal
                })
                .then((response) => {
                    let data = JSON.parse(response.data);
                    if (data.Status === "SUCCESS") {
                        self.setState({ cellCss: "border-success" });
                    } else {
                        toast.error(data.Result);
                        self.setState({ cellCss: "border-failed" });
                    }
                })
                .catch((error) => {

                    if (error.response !== undefined) {
                        let msg = error.message + ": " + error.response.statusText;
                        toast.error(msg);
                    } else {
                        toast.error(error.message);
                    }

                    self.setState({ cellCss: "border-failed" });
                });
        }
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
            s.filterRenderer = this.renderStringToFilter(s);
            s.formatter = this.renderStringToFormatter(s);
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

    renderControlToEditor = (ary) => {

        if (ary.control === "date" && ary.editable) {
            ary.editor = <DatePickerEditor />;
        }
        else if (ary.control.toLowerCase() === "boolean" && ary.editable) {
            ary.editor = <DropDownEditor options={BoolItem} />
        }
        else if (ary.control === "DropDown" && ary.editable) {
            ary.editor = <DropDownEditor options={ary.name === "ControlType" ? ControlItem : ''} />;
        }
        else if (ary.control === "role") {
            ary.editor = <DropDownEditor options={this.props.parentOptions} />;
        }

        return ary.editor;
    }

    renderStringToHeaderRenderer = (ary) => {

        if (ary.headerRenderer === undefined) {
            //if (ary.Group === undefined)
            //    ary.Group = "";

            ary.headerRenderer = <HeaderGroup parent={ary.Group} child={ary.name} isDoubleHeader={this.props.isDoubleHeader} />;
        }

        return ary.headerRenderer;
    }

    renderStringToFormatter = (ary) => {
        if (ary.control.toLowerCase() === "boolean")
        {
            ary.formatter = <BooleanFormatter />;
        }

        return ary.formatter;
    }

    onLoad = () => {
        const self = this;

        axios.get(this.props.getApi,
            {
                cancelToken: new CancelToken(function executor(c) {
                    cancel = c;
                })
            }).then((response) => {
                let data = response.data;
                let obj = JSON.parse(data);
                let _header = JSON.parse(obj.Header);
                let _rows = "";
                if (obj.Body !== "") {
                    _rows = JSON.parse(obj.Body);

                    for (let i = 0; i <= _rows.length - 1; i++) {
                        _rows[i].No = i + 1;
                    }
                }
                self.setState({ header: _header, rows: _rows });

            })
            .catch((error) => {

                if (error.response !== undefined) {
                    let msg = error.message + ": " + error.response.statusText;
                    toast.error(msg);
                }
            });

    }

    onDoubleClick = (rowIdx, row, column) => {
        if (column.control === "identity") {
            const self = this;
            let identifier = self.props.identifier(row, 1);

            axios.get("api/History/GetHistory",
                {
                    params: {
                        identifier: identifier
                    },
                    cancelToken: new CancelToken(function executor(c) {
                        cancel = c;
                    }),
                }
            ).then((response) => {
                let obj = JSON.parse(response.data);
                let _header = JSON.parse(obj.Header);
                let _rows = "";

                if (obj.Body !== "") {
                    _rows = JSON.parse(obj.Body);
                    for (let i = 0; i <= _rows.length - 1; i++) {
                        _rows[i].No = i + 1;
                        _rows[i].CreatedDate = moment(_rows[i].CreatedDate).format("DD/MM/YYYY hh:mm A");
                    }
                }

                self.setState({
                    isHistoryModal: true,
                    historyData: {
                        header: _header,
                        body: _rows
                    }
                });

            }).catch((error) => {
                let msg = error.message + ": " + error.response.statusText; 

                toast.error(msg);
            });
        }
    }

    onRowClick = (id, row, col) => {

        if (id >= 0 && col.control === "identity") {

            let header = this.state.header;
            let role = localStorage.getItem("role");
            let ary = [];
            for (var i = 0; i <= header.length - 1; i++) {
                let col = header[i];
                if (col.Group === role && col.inlineField) {
                    ary.push(col);
                }
            }
            this.setState({
                groupData: {
                    header: ary,
                    body: row
                },
                isGroupClicked: true
            })
        }
    }

    onGroupButtonClick = () => {
        this.setState({
            isGroupModal: true
        });
    }

    onGroupCloseRefresh = (refresh) => {
        let self = this;

        self.setState({
            isGroupModal: false
        });

        if (refresh) {
            self.onLoad();
        }
    }

    ToolbarButton = () => {

        let isMatch = false;
        let role = "";
        let roleColor = "";
        if (this.props.isGroupButton) {
            let header = this.state.header;
            role = localStorage.getItem("role");
            for (var i = 0; i <= header.length - 1; i++) {
                let singleHeader = header[i];

                if (singleHeader.Group === role) {
                    isMatch = true;
                    singleHeader.headerClass = (singleHeader.headerClass === undefined ? "steelblue" : singleHeader.headerClass);
                    roleColor = "grid-btn-left btn fo-white " + singleHeader.headerClass;
                    break;
                }
            }
        }

        return (
            <div>
                <div className="grid-btn-right" >
                    {this.props.gridButton}
                    <CsvButton header={this.state.header} body={this.state.rows} />
                </div>
                {
                    (this.props.isGroupButton && isMatch) &&
                    <button type="button" disabled={!this.state.isGroupClicked} className={roleColor} onClick={this.onGroupButtonClick}><i className="fa fa-pencil-square-o"></i> {role}</button>
                }
            </div>
        )
    }

    render() {
        let apiData = this.state.header;
        let isHistoryClose = () => this.setState({ isHistoryModal: false });

        if (apiData !== undefined && apiData.length >0) {
            apiData = this.renderStringToObject(apiData);
            return (
                <div>
                    <RowContext.Provider value={this.state.cellRow}>
                        <ColContext.Provider value={this.state.cellCol}>
                            <ColorContext.Provider value={this.state.cellCss}>
                                <ReactDataGrid
                                    onGridSort={this.handleGridSort}
                                    onRowClick={(id, row, col) => this.onRowClick(id, row, col)}
                                    enableCellSelect={true}
                                    columns={apiData}
                                    rowGetter={this.rowGetter}
                                    rowsCount={this.getSize()}
                                    headerRowHeight={this.props.isDoubleHeader?100:0}
                                    minHeight={this.state.height}
                                    cellNavigationMode="changeRow"
                                    onGridRowsUpdated={this.handleGridRowsUpdated}
                                    onRowDoubleClick={this.onDoubleClick}
                                    toolbar={<Toolbar enableFilter={true} children={this.ToolbarButton()} />}
                                    onAddFilter={this.handleFilterChange}
                                    getValidFilterValues={this.getValidFilterValues}
                                    onClearFilters={this.onClearFilters}
                                    emptyRowsView={EmptyRowFormatter}
                                    rowRenderer={RowRenderer}
                                />
                            </ColorContext.Provider>
                        </ColContext.Provider>
                    </RowContext.Provider>

                    {
                        this.state.isHistoryModal &&
                        <HistoryModal modal={this.state.historyData} show={this.state.isHistoryModal} onHide={isHistoryClose} />
                    }
                    {
                        this.props.isGroupButton &&
                        <GroupModal modal={this.state.groupData} show={this.state.isGroupModal} onHide={this.onGroupCloseRefresh} />
                    }
                   
                </div>
            )
        }

        return (<div>Loading....</div>);
    }
}

ReactGrids.propTypes = {
    getApi: PropTypes.string,
    isDoubleHeader: PropTypes.bool,
    isGroupButton: PropTypes.bool,
    gridButton: PropTypes.element,
    refreshGrid: PropTypes.bool,
    parentOptions: PropTypes.array,
    postApi: PropTypes.string,
    identifier: PropTypes.func
}

class HeaderGroup extends Component {
    render() {
        if ((this.props.parent !== undefined) && this.props.isDoubleHeader) {
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
                    <div>{this.props.child}</div>
                </div>
            )
        }
    }
}

class CellRenderer extends Component {
    setScrollLeft = (scrollBy) => {
        // if you want freeze columns to work, you need to make sure you implement this as apass through
        this.row.setScrollLeft(scrollBy);
    };

    render() {
        //let isIdentity = (this.props.column.control === "identity");
       
        return (
            <RowContext.Consumer>
                {row => (
                    <ColContext.Consumer>
                        {col => (
                            <ColorContext.Consumer>
                                {color => (
                                    <Cell {...this.props} ref={node => this.row = node} className={(row === JSON.stringify(this.props.rowData) && col === this.props.column.key) ? color : ''} />
                                )}
                            </ColorContext.Consumer>
                        )}
                    </ColContext.Consumer>
                )}
            </RowContext.Consumer>
        );
    }
}

class RowRenderer extends Component {

    setScrollLeft = (scrollBy) => {
        this.row.setScrollLeft(scrollBy);
    };

    render() {
        return <Row cellRenderer={CellRenderer} ref={node => this.row = node} {...this.props} />;
    }
}
