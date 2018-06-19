import React from 'react';
import ReactDOM from 'react-dom';

const { editors: { EditorBase } } = require('react-data-grid');

export class InputEditor extends EditorBase {
    constructor(props) {
        super(props);
    }

    getInputNode() {
        return ReactDOM.findDOMNode(this);
    }

    getValue = () => {
        return {
            [this.props.column.key]: this.state.value
        };
    }

    onClick() {
        this.getInputNode().focus();
    }

    onDoubleClick() {
        this.getInputNode().focus();
    }

    onBlur = (value) => {

       
    }

    handleSelectChange = (value) => {
        
    }

    render() {
        return (
            <input type="text" className="form-control" onBlur={this.onBlur()} />
        )
    }
}