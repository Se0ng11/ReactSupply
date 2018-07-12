import React from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import { asyncContainer, Typeahead, Highlighter } from 'react-bootstrap-typeahead';
import { Row, Col, Modal, Button, FormGroup, FormControl, ControlLabel } from 'react-bootstrap';
const AsyncTypeahead = asyncContainer(Typeahead);

export class UserModal extends React.Component {
    name = "UserModal";

    constructor(props) {
        super(props);

        this.state = {
            color: "fo-green",
            message: "",
            userId: "",
            email: "",
            isLoading: false,
            options: [],
            rolesOption: [],
            role: "",
            appId: "",
            adLink: ""
        }
    }

    componentDidMount() {
        let self = this;

        axios.get('api/Settings/GetAdSearch')
            .then((response) => {
                let data = JSON.parse(response.data);
                self.setState({
                    appId: data.appId.Value,
                    adLink: data.adAuth.Value
                });

            }).catch((error) => {
                self.onError(error.message);
            });
    }

    handleSubmit = () => {
        const self = this;
        axios.post("/api/Users/Register",
            {
                UserName: self.state.userId,
                Email: self.state.email,
                Role: self.state.role
            }
        ).then((response) => {
            let data = JSON.parse(response.data);

            self.setState({
                color: (data.Status === "SUCCESS") ? 'fo-green' : 'fo-red',
                message: data.Result
            });

            if (self.state.color === "fo-green") {
                self.setState({
                    userId: "",
                    email: "",
                    role: ""
                });
                self._typeahead.getInstance().clear();
            } 

        }).catch((error) => {
            
            let msg = error.message + ": " + error.response.statusText;
            toast.error(msg);
        });
    }

    handleFocus = () => {
        this.setState({
            message: ""
        });
    }

    handleEntered = () => {
        axios.get("/api/Roles/GetRoles").then((response) => {
            let data = JSON.parse(response.data);

            if (data.Status === "SUCCESS") {
                let items = [];
                let options = JSON.parse(data.Data);
                items.push(<option key={-1} value={""}>{"Please select"}</option>);
                for (let i = 0; i <= options.length - 1; i++) {
                    items.push(<option key={i} value={options[i]}>{options[i]}</option>);
                }

                this.setState({
                    rolesOption: items
                })
            }

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

    handleHide = () => {
        this.setState({
            color: "fo-green",
            message: "",
            userId: "",
            email: "",
            isLoading: false,
            options: [],
            rolesOption: [],
            role: ""
        });

        this.props.onHide();
    }

    handleUserSearch = (query) => {
        let self = this;
        self.setState({ isLoading: true });
        axios.post(self.state.adLink,
            {
                AppToken: self.state.appId,
                UserToken: localStorage.getItem("adToken"),
                AppMethod: "searchad",
                AppParameter: query
            }
        ).then((response) => {
            let data = JSON.parse(response.data.Data);
            data = (data === null) ? [] : data;
            this.setState({
                isLoading: false,
                options: data
            });
        })
        .catch((error) => {

        });
    }

    onTypeAheadChange = (selected) => {
        if (selected.length > 0) {
            this.setState({
                email: selected[0].Email,
                userId: selected[0].UserId
            });
        } else {
            this.setState({
                email: "",
                userId: ""
            });
        }
    }

    renderMenuItemChildren(option, props, index) {
        return (
            <div>
                <Highlighter key="name" search={props.text}>
                    {option.UserId}
                </Highlighter>
                <div key="details">
                    <small>
                        <ul>
                            <li>Name: {option.Name}</li>
                            <li>Email: {option.Email}</li>
                        </ul>
                    </small>
                </div>
                <div className="divider"></div>
            </div>
        )
    }

    getValidationState() {
        //const length = this.state.userId.length;
        //if (length > 10) return 'success';
        //else if (length > 5) return 'warning';
        //else if (length > 0) return 'error';
        return null;
    }

    render() {
        return (
            <Modal
                {...this.props}
                onEntered={this.handleEntered}
                aria-labelledby="contained-modal-title-lg">
                <Modal.Header closeButton>
                    <Modal.Title id="contained-modal-title-lg">Add User</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <form>
                        <Row>
                            <Col xs={12} md={12}>
                                <FormGroup controlId="formControlsText"
                                    validationState={this.getValidationState()}
                                >
                                    <ControlLabel>User Id</ControlLabel>
                                    <AsyncTypeahead
                                        isLoading={false}
                                        onSearch={this.handleUserSearch}
                                        options={this.state.options}
                                        placeholder="Search for Hartalega Id..."
                                        labelKey="UserId"
                                        renderMenuItemChildren={this.renderMenuItemChildren}
                                        ref={(ref) => this._typeahead = ref}
                                        onChange={this.onTypeAheadChange}
                                        onFocus={this.handleFocus}
                                    />
                                </FormGroup>
                                <FormGroup controlId="formControlsSelect">
                                    <ControlLabel>Role</ControlLabel>
                                    <FormControl componentClass="select" placeholder="select" name="role" onFocus={this.handleFocus} onChange={this.handleChange} value={this.state.role}>
                                        {this.state.rolesOption}
                                    </FormControl>
                                </FormGroup>
                                <FormGroup>
                                    <ControlLabel>Email</ControlLabel>
                                    <FormControl.Static>{this.state.email}</FormControl.Static>
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