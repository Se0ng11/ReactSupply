import '../dataTable/ReactGrids.css';
import React, { Component } from 'react';
import ReactDataGrid, { Row, Cell } from 'react-data-grid';
import axios from 'axios';
import update from 'immutability-helper';
import PropTypes from 'prop-types';
import { DatePickerEditor } from './Editors';
import { BooleanFormatter, ModalFormatter, EmptyRowFormatter } from './Formatter';
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
            isModal: false,
            modalData: {
                header: [],
                body:[]
            }
        };

        this.onClickShowModal = this.onClickShowModal.bind(this);
    }

    componentDidMount() {
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
                let _rows = JSON.parse(obj.Body);

                for (let i = 0; i <= _rows.length - 1; i++) {
                    _rows[i].No = i + 1;
                }

                self.setState({ header: _header, rows: _rows });
             
            })
            .catch((error) => {

                if (error.response !== undefined) {
                    let msg = "ReactGrids() " + error.message + ": " + error.response.statusText;
                    toast.error(msg);
                }
            });
    }

    componentWillUnmount() {
        cancel("User cancel it");
    }

    rowGetter = (rowIdx) => {
        let rows = this.getRows();

        return rows[rowIdx];
    };

    handleGridRowsUpdated = ({ cellKey, fromRow, toRow, updated, action, originRow }) => {
        let rows = Selectors.getRows(this.state);
        var self = this;

        if (self.state.cellCss === "border-success") {
            self.setState({ cellCss: "" });
        }
       
        for (let i = fromRow; i <= toRow; i++) {
            let rowToUpdate = rows[i];
            let updatedRow = update(rowToUpdate, { $merge: updated });
  
            if (JSON.stringify(rowToUpdate) !== JSON.stringify(updatedRow))
            {
                self.setState({ cellCss: "border-progress" });
                rows[i] = updatedRow;

                this.setState({
                    cellRow: JSON.stringify(updatedRow),
                    cellCol: cellKey
                })

                if (this.props.isBasic)
                {
                    this.postToServer(updatedRow.ValueName, JSON.stringify(updated));
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
        const self = this;
        if (this.props.postApi !== "") {
            axios.put(this.props.postApi,
                {
                    identifier: aX,
                    updated: vC
                })
                .then((response) => {
                    self.setState({ cellCss: "border-success" });
                })
                .catch((error) => {

                    if (error.response !== undefined) {
                        let msg = "ReactGrids() " + error.message + ": " + error.response.statusText;
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

        if (ary.control === "date") {
            ary.editor = <DatePickerEditor />;
        }
        else if (ary.control.toLowerCase() === "boolean") {
            ary.editor = <DropDownEditor options={BoolItem} />
        }
        else if (ary.control === "DropDown") {
            ary.editor = <DropDownEditor options={ary.name === "ControlType"? ControlItem : ''} />;
        }

        return ary.editor;
    }

    renderStringToHeaderRenderer = (ary) => {

        if (ary.headerRenderer === undefined) {
            if (ary.Group === undefined)
                ary.Group = "";

            ary.headerRenderer = <HeaderGroup parent={ary.title} child={ary.name} isDoubleHeader={this.props.isDoubleHeader} />;
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

    onDoubleClick = (rowIdx, row, column) => {
        if (column.control === "identity") {
            const self = this;

            axios.get("api/History/GetHistory",
                {
                    params: {
                        identifier: row.AX6SO
                    },
                    cancelToken: new CancelToken(function executor(c) {
                        cancel = c;
                    }),
                }
            ).then((response) => {
                let obj = JSON.parse(response.data);
                let _header = JSON.parse(obj.Header);
                let _rows = JSON.parse(obj.Body);

                self.setState({
                    isModal: true,
                    modaldata: {
                        header: _header,
                        body: _rows
                    }
                });

            }).catch((error) => {
                console.log(error);
                let msg = "postToServer() " + error.message + ": " + error.response.statusText; 

                toast.error(msg);
            });
        }
    }

    onClickShowModal = () => {
        const self = this;
        self.setState({
            isModal: true
        });
    }

    ToolbarButton = () => {
        return (
            <div className="grid-btn-left" >
                {this.props.gridButton}
              
                {
                    this.props.isCsvButton &&
                    <CsvButton header={this.state.header} body={this.state.rows} />
                }
            </div>
        )
    }

//     {
//    this.props.isRoleButton &&
//        <RoleButton onClick={this.props.onClick} />
//}
//{
//    this.props.isAddButton &&
//        <AddButton onClick={this.props.onClick} />
//}

    handleAddRow =() =>{

    }

    render() {
        let apiData = this.state.header;
        let isClose = () => this.setState({ isModal: false });

        if (apiData !== undefined) {
            apiData = this.renderStringToObject(apiData);
            return (
                <div>
                    <RowContext.Provider value={this.state.cellRow}>
                        <ColContext.Provider value={this.state.cellCol}>
                            <ColorContext.Provider value={this.state.cellCss}>
                                <ReactDataGrid
                                    onGridSort={this.handleGridSort}
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
                    <ModalFormatter show={this.state.isModal} onHide={isClose} />
                </div>
            )
        }

        return (<div>Loading....</div>);
    }
}

ReactGrids.propTypes = {
    getApi: PropTypes.string,
    postApi: PropTypes.string,
    isBasic: PropTypes.bool,
    isDoubleHeader: PropTypes.bool,
    isCsvButton: PropTypes.bool,
    gridButton: PropTypes.element
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
