import React from 'react';
import ReactDataGrid from 'react-data-grid';
import { Modal, Button } from 'react-bootstrap';

import { EmptyRowFormatter } from '../../Formatter';

const { Data: { Selectors } } = require('react-data-grid-addons');

export class HistoryModal extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            header: this.props.modal.header,
            rows: this.props.modal.body,
            filters: {},
            sortColumn: null,
            sortDirection: null
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

    rowGetter = (rowIdx) => {
        let rows = this.getRows();

        return rows[rowIdx];
    };

    getSize = () => {
        return this.getRows().length;
    };

    getRows = () => {
        return Selectors.getRows(this.state);
    };


    render() {
        return (
            <div>
                <Modal
                    {...this.props}
                    bsSize="large"
                    dialogClassName="custom-modal"
                    aria-labelledby="contained-modal-title-lg">
                    <Modal.Header closeButton>
                        <Modal.Title id="contained-modal-title-lg">History</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <div>
                            <ReactDataGrid
                                columns={this.state.header}
                                onGridSort={this.handleGridSort}
                                rowGetter={this.rowGetter}
                                rowsCount={this.getSize()}
                                emptyRowsView={EmptyRowFormatter}
                                    minHeight={500} />
                        </div>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={this.props.onHide}>Close <i className="fa fa-sign-out"></i></Button>
                    </Modal.Footer>
                </Modal>

            </div>
        )
    }
}

//<ReactDataGrid
//    columns={this._columns}
//    rowGetter={this.rowGetter}
//    rowsCount={this._rows.length}
//    minHeight={200} />