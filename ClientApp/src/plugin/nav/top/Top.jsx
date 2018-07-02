import '../top/top.css';
import React from 'react';
import axios from 'axios';
import { Navbar, Nav, NavItem } from 'react-bootstrap';
import { Link, Redirect } from 'react-router-dom';
import { LinkContainer } from 'react-router-bootstrap';
import { toast } from 'react-toastify';

export default class Top extends React.Component {
    constructor() {
        super();

        this.state = {
            redirect: false
        };
    }

    onMenuChange = (code) => {
        localStorage.setItem("currentMenu", code);
    }

    onSignOut = () => {
        const self = this;
        axios.post("/api/Auth/Logout",
            {
                UserId: localStorage.getItem("user"),
                Refresh: localStorage.getItem("refresh")
            }
        ).then((response) => {
            localStorage.clear();
            self.setState({ redirect: true });
        })
        .catch((error) => {
            let msg = "Layout() " + error.message + ": " + error.response.statusText;
            toast.error(msg);
        });
    }

    generateLinkContainer = () => {
        const obj = this.props.menu.map((value, index) => 
            <LinkContainer to={value.Url} key={value.MenuCode}>
                <NavItem onClick={() => this.onMenuChange(value.MenuCode)}>
                    <i className={"fa " + value.MenuClass} aria-hidden="true"></i> {value.MenuName}
                </NavItem>
            </LinkContainer>
        );
        return obj;
    }

    render() {
        let user = localStorage.getItem('user');
        let isDirect = this.state.redirect;

        if (isDirect) {
            return <Redirect to='/' />;
        }

        return (
            <div>
                <Navbar fluid collapseOnSelect fixedTop className="shadow">
                    <Navbar.Header>
                        <Navbar.Brand>
                            <Link to="/Home">Sample</Link>
                        </Navbar.Brand>
                        <Navbar.Toggle />
                    </Navbar.Header>
                    <Navbar.Collapse>
                        <Nav>
                            <LinkContainer to={'/Home'}>
                                <NavItem onClick={()=> this.onMenuChange("0")}>
                                    <i className="fa fa-home" aria-hidden="true"></i> Home
                                </NavItem>
                            </LinkContainer>
                            {this.generateLinkContainer()}
                        </Nav>
                        <Navbar.Form pullRight>
                            <button type="button" className="btn btn-danger btn-block" onClick={() => this.onSignOut()}>Sign Out <i className="fa fa-sign-out"></i></button>
                        </Navbar.Form>
                        <div className="navbar-form navbar-right">
                            <h5><i className="fa fa-user" aria-hidden="true"></i> { user } </h5>
                        </div>
                    </Navbar.Collapse>
                </Navbar>
                {this.props.children}

            </div>
        );
    };
}
