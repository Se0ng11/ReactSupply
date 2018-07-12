import React from 'react';
import { Well } from 'react-bootstrap';

export class EmptyRowFormatter extends React.Component {
    render() {
        return (
            <div>
                <Well bsSize="large">No data</Well>
            </div>
        );
    }
}
