import React, { Component } from 'react';
import { Route } from 'react-router';
import { ReactGridSample } from './components/sample/SampleList';
import { Config } from './components/config/Config';
import Auth from './components/auth/Auth';

export default class App extends Component {
  displayName = App.name

  render() {
      return (
          <div>
                <Route exact path='/' component={Auth} />
                <Route path='/home' component={ReactGridSample} />
                <Route path='/config' component={Config} />
          </div>
    );
  }
}
