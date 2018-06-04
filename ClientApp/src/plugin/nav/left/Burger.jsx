import '../left/burger.css';
import React from 'react';
import axios from 'axios';
import { push as Menu } from 'react-burger-menu';
//import { LinkContainer } from 'react-router-bootstrap';
//import { Navbar, Nav, NavItem } from 'react-bootstrap';
import { Link } from "react-router-dom";

//import {Collapse, Button, Well } from 'react-bootstrap';

export default class Burger extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            menu: [],
            open: false
        }
    }

    componentDidMount() {
        const self = this;
        axios.get("api/Menu/GetSubMenuDisplay")
            .then((response) => {
                self.setState({ menu: JSON.parse(response.data) });
            })
            .catch((error) => {
                console.log(error);
            });
    }

    onClick = (value) => {
        console.log("click here");
    }

    generateSubContainer = () => {
        const obj = this.state.menu.map((value, index) => {
            if (localStorage.getItem("currentMenu") === value.MenuCode) {
                return (
                    <Link to={value.Url} key={value.SubCode}><i className={"fa " + value.SubClass} aria-hidden="true"></i> {value.SubName}</Link>
                )
            }
        });

        return obj;
    }

    render() {
        return (
            <Menu noOverlay pageWrapId={"page-wrap"}>
                {this.generateSubContainer()}
            </Menu>
        )
    }

}

//<ul>
//    <li>
//        <i className="fa fa-book"></i><span> Report 1</span>
//        <ul>
//            <li onClick={this.onClick}>
//                <i className="fa fa-file fo-red" aria-hidden="true"></i> Sub Report 1
//                            </li>
//            <li>
//                <i className="fa fa-file fo-green" aria-hidden="true"></i> Sub Report 2
//                            </li>
//            <li>
//                <i className="fa fa-file fo-blue" aria-hidden="true"></i> Sub Report 3
//                            </li>
//            <li>
//                <i className="fa fa-file fo-yellow" aria-hidden="true"></i> Sub Report 4
//                            </li>
//        </ul>
//    </li>
//    <li><i className="fa fa-book fo-green"></i><span> Report 2</span></li>
//    <li><i className="fa fa-file fo-red"></i><span> Report 3</span></li>
//</ul>

//<Button onClick={() => this.setState({ open: !this.state.open })}>
//    click
//                </Button>
//    <Collapse in={this.state.open}>
//        <div>
//            <Well>
//                Anim pariatur cliche reprehenderit, enim eiusmod high life
//                accusamus terry richardson ad squid. Nihil anim keffiyeh
//                helvetica, craft beer labore wes anderson cred nesciunt sapiente
//                ea proident.
//                        </Well>
//        </div>
//    </Collapse>