import './container.css';
import React from 'react';
import PropTypes from 'prop-types';

export default class Container extends React.Component {
    constructor(props) {
        super(props);

        let defaultCss = (props.isMinimize && !props.fixed) ? "search-close" : "";
        this.state = {
            isShow: props.fixed,
            css: defaultCss,
            icon: "fa fa-chevron-up"
        }
    }

    onClick = () => {
        let cssName = "";
        let isShow = false;
        let icon = "";

        if (this.state.isShow) {
            cssName = "search-close"
            isShow = false;
            icon = "fa fa-chevron-up";
        } else {
            cssName = "search-open";
            isShow = true;
            icon = "fa fa-chevron-down";
        }

        this.setState({
            css: cssName,
            isShow: isShow,
            icon: icon
        });

    }

    render() {
        let self = this;
        let title = self.props.title;
        let fixed = (self.props.fixed) ? "fixed" : "";
        return (
            <div className={"container-border " + self.state.css + " " + fixed}>
                {
                    title !== "" &&
                    <div className="search-title">{title}</div>
                }

                {
                    self.props.isMinimize &&
                    <div className="top-right"><button type="button" className="btn btn-default" onClick={self.onClick}><i className={self.state.icon}></i></button></div>
                }
                <hr />
                <div className="content">
                    {self.props.children}
                </div>
            </div>    
        );
    }
}

Container.propTypes = {
    title: PropTypes.string,
    isMinimize: PropTypes.bool,
    fixed: PropTypes.bool
}