import 'bootstrap/dist/css/bootstrap.css';
//import 'bootstrap/dist/css/bootstrap-theme.css';
import 'font-awesome/css/font-awesome.css';
import 'react-toastify/dist/ReactToastify.min.css';
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Route } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import NetworkService from './plugin/networkService/NetworkService';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

NetworkService.setupRequestInterceptors();
NetworkService.setupResponseInterceptors();

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
        <Route path="/" component={App} />
  </BrowserRouter>,
  rootElement);

registerServiceWorker();
