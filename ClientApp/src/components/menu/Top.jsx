import React, { Component } from 'react';
import { Navbar, Nav, NavItem } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { LinkContainer } from 'react-router-bootstrap';
import '../menu/top.css';

export default class Top extends Component {

    render() {
        return (
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
                            <NavItem>
                                <i className="fa fa-home"></i> React Grid
                            </NavItem>
                        </LinkContainer>
                        <LinkContainer to={'/reacttable'}>
                            <NavItem>
                                <i className="fa fa-table"></i> Logs
                            </NavItem>
                        </LinkContainer>
                        <LinkContainer to={'/reacttable'}>
                            <NavItem>
                                <i className="fa fa-line-chart"></i> Reports
                            </NavItem>
                        </LinkContainer>
                        <LinkContainer to={'/config'}>
                            <NavItem>
                                <i className="fa fa-tasks"></i> Configuration
                            </NavItem>
                        </LinkContainer>
                    </Nav>
                    <Navbar.Form pullRight>
                        <Link className="btn btn-danger btn-block" to='/'>Sign Out <i className="fa fa-sign-out"></i></Link>
                    </Navbar.Form>
                    <div className="navbar-form navbar-right">
                        <h5><i className="fa fa-user" aria-hidden="true"></i> Welcome, User</h5>
                    </div>
                </Navbar.Collapse>
            </Navbar>
        );
    };
}

//<li><Link to='/Home'><i className="fa fa-home"></i> Home</Link></li>
//    <li><Link to='/Home'><i className="fa fa-users"></i> Admin</Link></li>