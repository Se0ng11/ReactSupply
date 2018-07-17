import React from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import { Row, Col, Modal, Button, FormGroup, FormControl, ControlLabel } from 'react-bootstrap';

export class RoleModal extends React.Component {
    name = "RoleModal";

    constructor(props) {
        super(props);

        this.state = {
            color: "fo-green",
            message: "",
            isLoading: false,
            options: [],
            name: ""
        }
    }

    handleSubmit = () => {
        const self = this;
        axios.post("api/Roles/Register",
            {
                Name: self.state.name
            }
        ).then((response) => {
            let data = JSON.parse(response.data);

            self.setState({
                color: (data.Status === "SUCCESS") ? 'fo-green' : 'fo-red',
                message: data.Result
            });

        }).catch((error) => {
            let msg = error.message + ": " + error.response.statusText;
            toast.error(msg);
        });
    }

    handleChange = (e) => {
        this.setState({
            [e.target.name]: e.target.value
        });
    }

    handleFocus = () => {
        this.setState({
            message: ""
        });
    }


    handleHide = () => {
        this.setState({
            color: "fo-green",
            message: "",
            isLoading: false,
            options: []
        });

        this.props.onHide();
    }

    render() {
        return (
            <Modal
                {...this.props}
                aria-labelledby="contained-modal-title-lg">
                <Modal.Header closeButton>
                    <Modal.Title id="contained-modal-title-lg">Add Role</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <form>
                        <Row>
                            <Col xs={12} md={12}>
                                <FormGroup
                                    controlId="formBasicText"
                                >
                                    <ControlLabel>Role Name</ControlLabel>
                                    <FormControl
                                        type="text"
                                        name="name"
                                        placeholder="Enter Role Name"
                                        onChange={this.handleChange}
                                        onFocus={this.handleFocus}
                                    />
                                    <FormControl.Feedback />
                                </FormGroup>

                            </Col>
                        </Row>
                    </form>
                </Modal.Body>
                <Modal.Footer>
                    <div className="grid-btn-left"><h5 className={this.state.color}>{this.state.message}</h5></div>
                    <Button bsStyle="primary" onClick={this.handleSubmit} ><i className="fa fa-pencil"></i> Save</Button>
                    <Button onClick={this.handleHide}>Close <i className="fa fa-sign-out"></i></Button>
                </Modal.Footer>
            </Modal>
        );
    }
}