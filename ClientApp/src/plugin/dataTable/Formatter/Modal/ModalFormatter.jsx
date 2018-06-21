import React from 'react';
import { Modal, Button } from 'react-bootstrap';
import ReactDataGrid, { Row, Cell } from 'react-data-grid';

export class ModalFormatter extends React.Component {
    constructor(props, context) {
        super(props, context);
        this.createRows();
        this._columns = [
            { key: 'id', name: 'ID' },
            { key: 'title', name: 'Title' },
            { key: 'count', name: 'Count' },
            { key: 'aaa', name: 'aaa' }
        ];
        this.state = null;
    }

    createRows = () => {
        let rows = [];
        for (let i = 1; i < 5; i++) {
            rows.push({
                id: i,
                title: 'Title ' + i,
                count: i * 10,
                aaa: "test"
            });
        }

        this._rows = rows;
    };

    rowGetter = (i) => {
        return this._rows[i];
    };

    render() {
        return (
            <div>
                <Modal
                    {...this.props}
                    bsSize="large"
                    aria-labelledby="contained-modal-title-lg">
                    <Modal.Header closeButton>
                        <Modal.Title id="contained-modal-title-lg">History</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        
                        <ReactDataGrid
                            columns={this._columns}
                            rowGetter={this.rowGetter}
                            rowsCount={this._rows.length}
                            minHeight={200} />
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={this.props.onHide}>Close</Button>
                    </Modal.Footer>
                </Modal>

            </div>


        )
    }
}
