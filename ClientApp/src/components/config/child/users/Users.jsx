import React from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import ReactGrids from '../../../../plugin/dataTable/ReactGrids';
import { UserModal } from './UserModal';
import { RoleModal } from './RoleModal';

export class Users extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isUserModal: false,
            isRoleModal: false,
            refreshGrid: false,
            roleOptions: []
        }
    }

    componentDidMount = () => {
        this.onLoad();
    }

    onLoad = () => {
        axios.get("api/Roles/GetRoles").then((response) => {
            let data = JSON.parse(response.data);

            if (data.Status === "SUCCESS") {
                let items = [];
                let options = JSON.parse(data.Data);

                for (let i = 0; i <= options.length - 1; i++) {
                    items.push({ id: options[i], title: options[i], value: options[i] });
                };
                this.setState({
                    roleOptions: items
                })
            }

        }).catch((error) => {
            let msg = error.message + ": " + error.response.statusText;
            toast.error(msg);
        });
    }

    onClickToggleUserModal = (flag) => {
        this.setState({
            isUserModal: flag,
            refreshGrid: flag
        });
    }

    onClickToggleRoleModal = (flag) => {
        this.setState({
            isRoleModal: flag,
            refreshGrid: flag
        });
    }

    getIdentifier = (obj) => {
        return obj.UserName;
    }

    render() {
        if (this.state.roleOptions.length > 0) {
            return (
                <div>
                    <ReactGrids
                        headerRowHeight={50}
                        getApi="api/Users/GetUsers"
                        postApi="api/Users/PostRoles"
                        isHistoryEnabled={true}
                        refreshGrid={this.state.refreshGrid}
                        parentOptions={this.state.roleOptions}
                        identifier={this.getIdentifier}
                        gridButton={
                            <div className="grid-btn-left">
                                <RoleButton onClick={() => this.onClickToggleRoleModal(true)} />
                                <UserButton onClick={() => this.onClickToggleUserModal(true)} />
                            </div>
                        } />

                    <UserModal show={this.state.isUserModal} onHide={() => this.onClickToggleUserModal(false)} />
                    <RoleModal show={this.state.isRoleModal} onHide={() => this.onClickToggleRoleModal(false)} />
                </div>  
                );
        }
        return(<div></div>);
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