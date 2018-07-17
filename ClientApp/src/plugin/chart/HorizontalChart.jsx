import React from 'react';
import { HorizontalBar } from 'react-chartjs-2';

const data = {
    labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
    datasets: [
        {
            label: 'Sample data',
            backgroundColor: 'green',
            borderColor: 'green',
            borderWidth: 1,
            hoverBackgroundColor: 'lightgreen',
            hoverBorderColor: 'lightgreen',
            data: [65, 59, 80, 81, 56, 61, 59]
        }
    ]
};

export default class HorizontalChart extends React.Component {

    render() {
        return (
            <div>
                <h2>Horizontal Bar Example</h2>
                <HorizontalBar data={data} />
            </div>
        );
    }
}