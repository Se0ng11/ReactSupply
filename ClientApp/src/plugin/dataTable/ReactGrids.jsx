﻿import '../dataTable/ReactGrids.css';
import React, { Component } from 'react';
import ReactDataGrid, { Row, Cell } from 'react-data-grid';
import axios from 'axios';
import update from 'immutability-helper';
import PropTypes from 'prop-types';
import moment from 'moment';
import Container from '../container/Container';
import Loader from '../loader/Loader';
import { DatePickerEditor } from './Editors';
import { BooleanFormatter, DateFormatter, EmptyRowFormatter, HistoryModal, GroupModal } from './Formatter';
import { CsvButton } from './Button';
import { toast } from 'react-toastify';


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

        let realHeight = this.calculatePageHeight() - 220;

        this.state = {
            expanded: {},
            header: [],
            rows: [],   
            raw:[],
            filters: {},
            sortColumn: null,
            sortDirection: null,
            cellCss: "",
            cellRow: null,
            cellCol: null,
            height: realHeight,
            isHistoryModal: false,
            isGroupModal: false,
            isGroupClicked: false,
            isLoading: true,
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

    calculatePageHeight = () => {
        let body = document.body,
            html = document.documentElement;

        let height = Math.max(body.scrollHeight, body.offsetHeight,
            html.clientHeight, html.scrollHeight, html.offsetHeight);

        return height;
    }

    componentDidMount() {
        this.onLoad();
        this.setState({
            isLoading: true
        });
    }

    componentWillReceiveProps = (props) => {

        if (props.refreshGrid && !this.state.isLoading) {
            this.onLoad(props.parameters);
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

        let allRows = self.state.rows;

        for (var i = 0; i <= allRows.length - 1; i++) {
            let s = allRows[i];
            let updated = rows[0];

            let old1 = self.props.identifier(s, 1);
            let new1 = self.props.identifier(updated, 1);

            let old2 = self.props.identifier(s, 2);
            let new2 = self.props.identifier(updated, 2);


            if (old1 === new1 && old2 === new2)
            {
                allRows[i] = updated;
                break;
            }
        }
        self.setState({ allRows });
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
                        toast.error(data.Data);
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

            ary.headerRenderer = <HeaderGroup parent={ary.Group} child={ary.name} />;
        }

        return ary.headerRenderer;
    }

    renderStringToFormatter = (ary) => {
        if (ary.control.toLowerCase() === "boolean") {
            ary.formatter = <BooleanFormatter />;
        }
        else if (ary.formatter === "Date") {
            ary.getRowMetaData = (row) => row;
            ary.formatter = <DateFormatter />;
        }

        return ary.formatter;
    }

    onLoad = (param) => {
        let self = this;

        self.setState({
            isLoading: true,
            expanded: {}
        });
        axios.get(self.props.getApi,
            {
                params: {
                    identifier: localStorage.getItem("currentMenu"),
                    updated: param
                },
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

                    self.setState({ raw: _rows });

                    _rows = self.splitParentChild(_rows);

                    for (let i = 0; i <= _rows.length - 1; i++) {
                        _rows[i].No = i + 1;
                    }
                    _rows.filter(x => x.children !== undefined).forEach((v, i) => {
                        let child = v.children;

                        for (let x = 0; x <= child.length - 1; x++) {
                            child[x].No = v.No + "." + (x+1);
                        }
                    });
                }

                self.setState({
                    header: _header,
                    rows: _rows,
                    isLoading: false
                });

            })
            .catch((error) => {
                self.setState({ isLoading: false });
                if (error.response !== undefined) {
                    let msg = error.message + ": " + error.response.statusText;
                    toast.error(msg);
                }
            });

    }

    splitParentChild = (rows) => {
        let parents = rows.filter(x => x.Parent === undefined);
        const childs = rows.filter(x => x.Parent !== undefined);
        const unique = [...new Set(childs.map(x => x.Parent))];
        for (var i = 0; i <= unique.length - 1; i++) {
            parents = this.createChild(parents, childs, unique[i]);
        }
        return parents;
    }

    createChild = (parents, childs, unique) => {
        let currentRow = parents.find(x => x.Identifier === unique);

        currentRow.children = childs.filter(x => x.Parent === unique);
        return parents;
    }

    onDoubleClick = (rowIdx, row, column) => {
        const self = this;
        if (column.control === "identity" && self.props.isHistoryEnabled) {

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
                let _body = "";

                if (obj.Body !== "") {
                    _body = JSON.parse(obj.Body);
                    for (let i = 0; i <= _body.length - 1; i++) {
                        _body[i].No = i + 1;
                        _body[i].CreatedDate = moment(_body[i].CreatedDate).format("DD/MM/YYYY hh:mm A");
                    }
                }

                self.setState({
                    isHistoryModal: true,
                    historyData: {
                        header: _header,
                        body: _body
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
                isGroupClicked: ((row.children !== undefined || row.Parent !== undefined) && (row.ContainerTruckOutStatus === undefined || row.ContainerTruckOutStatus === "")) ? true: false
            })
        }
    }

    //Group Button
    onGroupButtonClick = () => {
        this.setState({
            isGroupModal: true
        });
    }

    onGroupCloseRefresh = (refresh, identifier, updated, child) => {
        let self = this;

        self.setState({
            isGroupModal: false,
            isGroupClicked: false
        });

        if (refresh) {
            let rows = self.state.rows;
            let affectedRow = "";

            if (child.length > 0) {

                for (var i = 0; i <= child.length - 1; i++) {
                    self.updateChild(self, identifier, updated, rows, child[i]);
                }
            } else {
                affectedRow = rows.find(x => x.Identifier === identifier);
                self.updateObject(affectedRow, updated);
            }

        }
    }

    updateChild = (self, identifier, updated, rows, child) => {
     
        let childRow = rows.find(x => x.Identifier === identifier).children.find(x => x.Identifier === child);
        self.updateObject(childRow, updated);
    }

    updateObject = (obj, updated) => {
        Object.keys(updated).forEach(function (key) {
            if (updated[key] !== undefined) {
                obj[key] = updated[key];
            }
        })
        return obj;
    }
    //Group Button

    //sub row
    getSubRowDetails = (rowItem) => {

        let isExpanded = this.state.expanded[rowItem.AXSO] ? this.state.expanded[rowItem.AXSO] : false;

        return {
            group: rowItem.children && rowItem.children.length > 0,
            expanded: isExpanded,
            children: rowItem.children,
            field: 'AXSO',
            treeDepth: rowItem.treeDepth || 0,
            siblingIndex: rowItem.siblingIndex,
            numberSiblings: rowItem.numberSiblings
        };
    }

    onCellExpand = (args) => {
        let rows = this.state.rows.slice(0);
        let rowKey = args.rowData.Identifier;
        let rowIndex = rows.indexOf(args.rowData);
        let subRows = args.expandArgs.children;

        let expanded = Object.assign({}, this.state.expanded);
        if (expanded && !expanded[rowKey]) {
            expanded[rowKey] = true;
            this.updateSubRowDetails(subRows, args.expandArgs.treeDepth);
            rows.splice(rowIndex + 1, 0, ...subRows);
        } else if (expanded[rowKey]) {
            expanded[rowKey] = false;
            rows.splice(rowIndex + 1, subRows.length);
        }

        this.setState({ expanded: expanded, rows: rows });
    }

    updateSubRowDetails = (subRows, parentTreeDepth) => {
        let treeDepth = parentTreeDepth || 0;
        subRows.forEach((sr, i) => {
            sr.treeDepth = treeDepth + 1;
            sr.siblingIndex = i;
            sr.numberSiblings = subRows.length;
        });
    };

    //sub row
    ToolbarButton = () => {

        let isMatch = false;
        let role = "";
        let roleColor = "";
        if (this.props.isGroupButton) {
            let header = this.state.header;
            role = localStorage.getItem("role");
            for (var i = 0; i <= header.length - 1; i++) {
                let singleHeader = header[i];

                if (singleHeader.Group === role && singleHeader.inlineField) {
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
                    <CsvButton header={this.state.header} body={this.state.raw} />
                </div>
                {
                    (this.props.isGroupButton && isMatch) &&
                    <button type="button" disabled={!this.state.isGroupClicked} className={roleColor} onClick={this.onGroupButtonClick}><i className="fa fa-pencil-square-o"></i> {role}</button>
                }
                <label>Total record(s): {this.state.raw.length}</label>
            </div>
        )
    }

    render() {
        let apiData = this.state.header;
        let isHistoryClose = () => this.setState({ isHistoryModal: false });
        let headerHeight = this.props.headerRowHeight;

        if (apiData !== undefined && apiData.length >0) {
            apiData = this.renderStringToObject(apiData);
            return (
                <div>
                    {
                        this.props.search !== undefined &&
                        <Container title="Search" isMinimize={true}>
                            {this.props.search}
                        </Container>
                    }
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
                                    headerRowHeight={headerHeight}
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
                                    getSubRowDetails={this.getSubRowDetails}
                                    onCellExpand={this.onCellExpand}
                                />
                            </ColorContext.Provider>
                        </ColContext.Provider>
                    </RowContext.Provider>
                    <Loader show={this.state.isLoading} />
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

        return (
            <div>
                <Loader show={this.state.isLoading} />
            </div>

        );
    }
}

ReactGrids.propTypes = {
    headerRowHeight: PropTypes.number,
    getApi: PropTypes.string,
    isGroupButton: PropTypes.bool,
    isHistoryEnabled: PropTypes.bool,
    gridButton: PropTypes.element,
    refreshGrid: PropTypes.bool,
    parentOptions: PropTypes.array,
    postApi: PropTypes.string,
    identifier: PropTypes.func,
    search: PropTypes.element,
    parameters: PropTypes.string
}

class HeaderGroup extends Component {
    render() {
        return (
            <div>
                <p>{this.props.parent}</p>
                <p>{this.props.child}</p>
            </div>
        )
    }
}

class CellRenderer extends Component {
    setScrollLeft = (scrollBy) => {
        // if you want freeze columns to work, you need to make sure you implement this as apass through
        this.row.setScrollLeft(scrollBy);
    };

    render() {
        //let isIdentity = (this.props.column.control === "identity");
        let self = this;

        if (self.props.column.control === "date") {
            let currentDate = self.props.value;
            let targetDate = self.props.rowData[self.props.column.formatter];

            if (self.props.column.formatter !== undefined && (currentDate !== undefined && targetDate !== undefined)) {
                let mCurrent = moment(currentDate);
                let mTarget = moment(targetDate);

                if (mCurrent > mTarget) {
                    return (
                        <Cell {...self.props} ref={node => self.row = node} className="due-date" />
                    );
                }
            }
        }
       
        return (
            <RowContext.Consumer>
                {row => (
                    <ColContext.Consumer>
                        {col => (
                            <ColorContext.Consumer>
                                {color => (
                                    <Cell {...self.props} ref={node => self.row = node} className={(row === JSON.stringify(self.props.rowData) && col === self.props.column.key) ? color : ''} />
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
