import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ReactTableSample, ReactGridSample } from './components/sample/SampleList';
import Auth from './components/auth/Auth';

export default class App extends Component {
  displayName = App.name

  render() {
      return (
          <div>
                <Route exact path='/' component={Auth} />
                <Route path='/home' component={ReactGridSample} />
                <Route path='/reacttable' component={ReactTableSample} />
          </div>
    );
  }
}
