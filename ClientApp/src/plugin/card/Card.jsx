import './card.css';
import React from 'react';
import { Well } from 'react-bootstrap';
import PropTypes from 'prop-types';

export default class Card extends React.Component {

    generateColor = () => {
        let self = this;
        let number = parseFloat(self.props.value);
        let color = "green";

        if (number <= 20.9) {
            color = "green";
        }
        else if (number > 20.0 && number <= 60.9) {
            color = "yellow";
        }
        else if (number >= 61.0) {
            color = "red";
        }

        return color;
    }

    render() {
        let self = this;
        let color = self.generateColor();
        return (
            <Well bsClass="white-card">
                <div className={"icon " + color}>
                    <i className={"fa fa-4x fa-" + self.props.icon } aria-hidden="true"></i>
                </div>
                <div className="header">{self.props.name}</div>
                <hr />
                <div className="result">{self.props.value}%</div>
            </Well>
        );
    }

}

Card.propTypes = {
    name: PropTypes.string,
    color: PropTypes.string,
    icon: PropTypes.string,
    value: PropTypes.string
}