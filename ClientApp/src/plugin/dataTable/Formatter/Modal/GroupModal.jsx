import 'react-datepicker/dist/react-datepicker.css';
import React from 'react';
import axios from 'axios';
import DatePicker from 'react-datepicker';
import moment from 'moment';
import { toast } from 'react-toastify';
import { Modal, Button, FormGroup, FormControl, ControlLabel, Table, Checkbox } from 'react-bootstrap';

export class GroupModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            ui: {
                color: "",
                message: "",
                titleColor: "",
                title: "",
                refresh: false
            },
            child:[],
            post: null,
            archived: null
        };
    }

    componentDidMount = () => {

    }

    componentWillReceiveProps = (obj) => {
        var self = this;

        if (obj.show && obj.modal.header.length > 0) {
            let header = obj.modal.header;
            let col = header[0];
            let body = obj.modal.body;
            let children = body.children;
            let ary = [];

            if (children !== undefined) {
                for (let i = 0; i <= children.length-1; i++)
                {
                    ary.push(children[i].Identifier);
                }
            }

            self.setState(prevState=>({
                ui: {
                    ...prevState.ui,
                    titleColor: col.headerClass,
                    title: col.Group
                }, 
                child: ary,
                archived: null
            }));

            for (var i = 0; i <= header.length - 1; i++) {
                let s = header[i];

                self.setState({ [s.key]: body[s.key] });
            }
        }
    }

    generateFields = () => {
        let self = this;
        let header = self.props.modal.header;
        let body = self.props.modal.body;

        const fields = header.map((val) =>
            <div key={val.key}>
                {
                    (val.control === "date" && val.inlineField) &&
                    <FormGroup controlId={val.key}>
                        <ControlLabel>{val.name}</ControlLabel>
                        <DatePicker
                            name={val.key}
                            showMonthDropdown
                            showYearDropdown
                            dropdownMode="select"
                            todayButton={"Today"}
                            minDate={moment()}
                            maxDate={moment().add(2, 'years')}
                            className="form-control"
                            selected={self.handleDateSelected(self.state[val.key])}
                            onChange={self.handleDateChange.bind(self, val.key)}
                            onFocus={self.handleFocus}
                            dateFormat="YYYY/MM/DD"
                            readOnly={true}
                        />
                    </FormGroup>
                }

                {
                    (val.control === "input" && val.inlineField) &&
                    <FormGroup controlId={val.key}>
                        <ControlLabel>{val.name}</ControlLabel>
                        <FormControl
                            type="text"
                            name={val.key}
                            onChange={self.handleChange}
                            onFocus={self.handleFocus}
                            value={self.state[val.key]}
                        />
                    </FormGroup>
                }

                {
                    (val.control === "boolean" && val.inlineField) &&
                    <FormGroup controlId={val.key}>
                        <ControlLabel>{val.name}</ControlLabel>
                        <FormControl
                            componentClass="select"
                            placeholder="select" name={val.key}
                            onChange={self.handleChange}
                            onFocus={self.handleFocus}
                            value={self.state[val.key]}
                        >
                            <option value="">Please select</option>
                            <option value="true">true</option>
                            <option value="false">false</option>
                        </FormControl>
                    </FormGroup>
                }

            </div>
        );

        let item = [];

        if (body.Parent === undefined) {
            item = [...new Set(body.children.map(item => item.ITEMID))];
        }

        return (
            <div>
                <Table bordered>
                    <tbody>
                        <tr>
                            <th>AX SO</th>
                            <td>{body.AXSO}</td>
                            <th>SO Issued Date</th>
                            <td>{body.CREATEDDATE}</td>
                        </tr>
                        <tr>
                            <th>Customer References</th>
                            <td colSpan="3">{body.CUSTOMERREF}</td>
                        </tr>
                        {
                            body.Parent === undefined &&
                            <tr>
                                <th>
                                    <label>Size</label>
                                </th>
                                <td colSpan="3">
                                    {
                                        item.map((val) =>
                                            <div key={val} className="checkbox-group">
                                                <div className="title">
                                                    {val}
                                                </div>

                                                {body.children.filter(x => x.ITEMID === val).map((val) =>
                                                    <Checkbox inline
                                                        name="child"
                                                        key={val.Identifier}
                                                        onLoad={self.onCheckboxLoad}
                                                        onChange={self.handleChecked}
                                                        onFocus={self.handleFocus}
                                                        disabled={!(val.ContainerTruckOutStatus === undefined || val.ContainerTruckOutStatus === "")}
                                                        defaultChecked={true}
                                                        value={val.Identifier}>
                                                        {val.SIZE}
                                                    </Checkbox>
                                                )}
                                            </div>
                                        )
                                    }
                                </td>
                            </tr>
                        }
                    </tbody>
                </Table>
                {fields}
            </div>
        );
    }

    handleDateSelected = (target) => {
        if (target === undefined) {
            return null;
        }

        return moment(target);
    }

    handleDateChange = (target, value) => {
        let self = this;
        let date = value.format("YYYY/MM/DD");

        self.setState(prevState => ({
            [target]: date,
            post: {
                ...prevState.post,
                [target]: date
            }
        }));
    }

    handleChange = (e) => {
        let self = this;
        let name = e.target.name;
        let value = e.target.value;

        self.setState(prevState=> ({
            [name]: value,
            post: {
                ...prevState.post,
                [name]: value
            }
        }));
    }

    handleChecked = (e) => {
        let self = this;
        let value = e.target.value;
        let checked = [...self.state.child];

        if (e.target.checked)
        {
            checked.push(value);
        }
        else
        {
            checked = checked.filter(x => x !== value);
        }

        self.setState(prevState => ({
            child: checked
        }));

    }

    handleFocus = (e) => {
        let self = this;

        self.setState(prevState=> ({
            ui: {
                ...prevState.ui,
                message: ""
            }
        }));
    }

    handleSubmit = (e) => {
        const self = this;
        let child = self.state.child;
        let rawChild = self.props.modal.body.children;

        if (self.state.post !== null) {

            if ((rawChild !== undefined && child.length === 0)) {
                self.setState(prevState => ({
                    ui: {
                        ...prevState.ui,
                        color: 'fo-red',
                        message: "Please choose at least one size to update"
                    }
                }));
            } else {
                let id = "";

                if (child.length > 0) {
                    let temp = "";
                    for (let i = 0; i <= child.length - 1; i++) {
                        temp += child[i] + "|";
                    }
                    temp = temp.substring(0, temp.lastIndexOf("|"));
                    id = temp;
                }
                else {
                    id = self.props.modal.body.Identifier;
                }

                axios.post("api/Home/PostSupplyRecords",
                    {
                        identifier: id,
                        identifier1: "",
                        updated: JSON.stringify(self.state.post)
                    })
                    .then((response) => {
                        let data = JSON.parse(response.data);

                        if (data.Status === "SUCCESS") {

                            self.setState({
                                archived: self.state.post
                            });

                            self.setState(prevState => ({
                                ui: {
                                    ...prevState.ui,
                                    color: 'fo-green',
                                    message: data.Data,
                                    refresh: true
                                },
                                post: null
                            }));

                        } else {
                            self.setState(prevState => ({
                                ui: {
                                    ...prevState.ui,
                                    color: 'fo-red',
                                    message: data.Data
                                }
                            }));
                        }
                    })
                    .catch((error) => {

                        if (error.response !== undefined) {
                            let msg = error.message + ": " + error.response.statusText;
                            toast.error(msg);
                        } else {
                            toast.error(error.message);
                        }
                    });
            }
        }
        else
        {
            self.setState(prevState => ({
                ui: {
                    ...prevState.ui,
                    color: 'fo-red',
                    message: "Nothing to update"
                }
            }));
        }
    }


    handleHide = () => {
        let self = this;
        self.props.onHide(self.state.ui.refresh, self.props.modal.body.Identifier, self.state.archived, self.state.child);
        self.setState(prevState => ({
            ui: {
                ...prevState.ui,
                message: "",
                refresh: false,
            },
            child: []
        }));
    }

    render() {
        let self = this;

        if (self.props.modal.header.length > 0 && self.props.show) {
            let body = self.props.modal.body;
            return (
                    <Modal 
                    {...self.props}
                    backdrop="static"
                    onHide={self.handleHide}
                    >
                    <Modal.Header className={self.state.ui.titleColor} closeButton>
                        <Modal.Title className="fo-white">{self.state.ui.title + "(No: " + body.No + ")"}</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                        {self.generateFields()}
                        </Modal.Body>
                        <Modal.Footer>
                            <div className="grid-btn-left"><h5 className={self.state.ui.color}>{self.state.ui.message}</h5></div>
                             <Button bsStyle="primary" onClick={self.handleSubmit} ><i className="fa fa-pencil"></i> Update</Button>
                            <Button onClick={self.handleHide}>Close <i className="fa fa-sign-out"></i></Button>
                        </Modal.Footer>
                    </Modal>
            );
        }
        
        return (<div></div>);
    }

}