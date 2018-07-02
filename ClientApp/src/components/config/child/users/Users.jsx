import React from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import { asyncContainer, Typeahead, Highlighter} from 'react-bootstrap-typeahead';
import { Row, Col, Modal, Button, FormGroup, FormControl, ControlLabel } from 'react-bootstrap';
import ReactGrids from '../../../../plugin/dataTable/ReactGrids';

const AsyncTypeahead = asyncContainer(Typeahead);

export class Users extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isUserModal: false,
            isRoleModal: false
        }

        this.onClickShowAddUser = this.onClickShowAddUser.bind(this);
        this.onClickShowAddRole = this.onClickShowAddRole.bind(this);
    }

    onClickShowAddUser = () => {
        const self = this;
        self.setState({
            isUserModal: true
        });
    }

    onUserModalClose = () => {
        this.setState({
            isUserModal: false
        });
    }

    onClickShowAddRole = () => {
        const self = this;
        self.setState({
            isRoleModal: true
        });
    }

    onRoleModalClose = () => {
        this.setState({
            isRoleModal: false
        });
    }

    render() {
        return (
            <div>
                <ReactGrids
                    getApi="api/Users/GetUsers"
                    isAddButton={true}
                    isRoleButton={true}
                    isBasic={true}
                    isDoubleHeader={false}
                    gridButton={<div><RoleButton onClick={this.onClickShowAddRole} /><UserButton onClick={this.onClickShowAddUser} /></div> }
                    onClick={this.onClickShowAddUser} />

                <UserModal show={this.state.isUserModal} onHide={this.onUserModalClose} />
                <RoleModal show={this.state.isRoleModal} onHide={this.onRoleModalClose} />
            </div>  
        );
    }
}

class RoleButton extends React.Component {
    render() {
        return (
            <div className="grid-btn-left">
                <button type="button" className="btn" onClick={this.props.onClick}> <i className="fa fa-id-badge"></i> Role</button>
            </div>
        )
    }
}

class UserButton extends React.Component {
    render() {
        return (
            <div className="grid-btn-left">
                <button type="button" className="btn" onClick={this.props.onClick}> <i className="fa fa-user"></i> User</button>
            </div>
        )
    }
}

class RoleModal extends React.Component {
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
        axios.post("/api/Users/PostRole",
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
            let msg = "Role Modal() " + error.message + ": " + error.response.statusText;
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

class UserModal extends React.Component {
    name = "UserModal";

    constructor(props) {
        super(props);

        this.state = {
            color: "fo-green",
            message: "",
            userId: "",
            email: "",
            isLoading: false,
            options: []
        }
    }

    componentDidMount() {
        //fire to get roles
    }

    handleSubmit = () => {
        const self = this;
        axios.post("/api/Users/Register",
            {
                UserId: self.state.userId,
                Email: self.state.email
            }
        ).then((response) => {
            let data = JSON.parse(response.data);
         
            self.setState({
                color: (data.Status === "SUCCESS") ? 'fo-green' : 'fo-red',
                message: data.Result
            });
        }).catch((error) => {
            let msg = "User Modal() " + error.message + ": " + error.response.statusText;
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
            options: []
        });

        this.props.onHide();
    }

    handleUserSearch = (query) => {
        this.setState({ isLoading: true });
        axios.post("http://ngc-devvm1:8086/api/data",
            {
                AppToken: "Rr0eExUJ0zlvXr02",
                UserToken: localStorage.getItem("adToken"),
                AppMethod: "searchad",
                AppParameter: query
            }
        ).then((response) => {
            let data = JSON.parse(response.data.Data);
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
        const length = this.state.userId.length;
        if (length > 10) return 'success';
        else if (length > 5) return 'warning';
        else if (length > 0) return 'error';
        return null;
    }

    render() {
        return (
            <Modal
                {...this.props}
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
                                        onChange={this.onTypeAheadChange}
                                    />
                                </FormGroup>
                                <FormGroup controlId="formControlsSelect">
                                    <ControlLabel>Role</ControlLabel>
                                    <FormControl componentClass="select" placeholder="select">
                                        <option value="select">select</option>
                                        <option value="other">...</option>
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