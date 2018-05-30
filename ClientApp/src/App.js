import React, { Component } from 'react';
import { Route } from 'react-router';
import { ReactGridSample } from './components/sample/SampleList';
import { Config } from './components/config/Config';
import { Index } from './components/home/index';
import Auth from './components/auth/Auth';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import { Switch } from 'react-router-dom';
import { Layout } from './components/Layout';

const PageFade = (props) => (
    <CSSTransition
        {...props}
        classNames="fadeTranslate"
        timeout={500}
        mountOnEnter={true}
        unmountOnExit={true}
    />
)

export default class App extends Component {    

    render() {
        let locationKey = this.props.location.pathname;

        return (
            <Layout location={locationKey}>
                <TransitionGroup>
                    <PageFade key={locationKey}>
                        <Switch location={this.props.location}>
                            <Route exact path='/' component={Auth} />
                            <Route path='/home' component={Index} />
                            <Route path='/reactgrid' component={ReactGridSample} />
                            <Route path='/config' component={Config} />
                        </Switch>
                    </PageFade>
                </TransitionGroup>
            </Layout>
        );
    }
}
