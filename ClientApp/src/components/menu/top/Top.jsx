import '../top/top.css';
import React from 'react';
import { Navbar, Nav, NavItem } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { LinkContainer } from 'react-router-bootstrap';
import axios from 'axios';

export default class Top extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            menu:[]
        }
    }

    componentDidMount() {
        const self = this;
        axios.get("api/Menu/GetMenuDisplay")
            .then((response) => {
                self.setState({ menu: JSON.parse(response.data) });
            })
            .catch((error) => {
                console.log(error);
            });
    }

    onMenuChange = (code) => {
        localStorage.setItem("currentMenu", code);
    }

    onSignOut = () => {
        localStorage.clear();
    }

    generateLinkContainer = () => {
        
        const obj = this.state.menu.map((value, index) =>
            <LinkContainer to={value.Url} key={value.MenuCode}>
                <NavItem onClick={() => this.onMenuChange(value.MenuCode)}>
                    <i className={"fa " + value.MenuClass} aria-hidden="true"></i> {value.MenuName}
                </NavItem>
            </LinkContainer>
        );
        return obj;
    }

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
                            <NavItem onClick={()=> this.onMenuChange("0")}>
                                <i className="fa fa-home" aria-hidden="true"></i> Home
                            </NavItem>
                        </LinkContainer>
                        {this.generateLinkContainer()}
                    </Nav>
                    <Navbar.Form pullRight>
                        <Link className="btn btn-danger btn-block" to='/' onClick={()=> this.onSignOut()}>Sign Out <i className="fa fa-sign-out"></i></Link>
                    </Navbar.Form>
                    <div className="navbar-form navbar-right">
                        <h5><i className="fa fa-user" aria-hidden="true"></i> Welcome, User</h5>
                    </div>
                </Navbar.Collapse>
            </Navbar>
        );
    };
}
