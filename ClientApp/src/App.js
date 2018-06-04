import React, { Component } from 'react';
import { Route } from 'react-router';
import { Switch } from 'react-router-dom';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import { Layout } from './components/Layout';
import { Auth, Home, Sample, Config, NotFound, Report } from './components/MenuList';

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
                            <Route path='/home' component={Home} />
                            <Route path='/reactgrid' component={Sample} />
                            <Route path='/report' component={Report} />
                            <Route path='/config' component={Config} />
                            <Route component={NotFound} />
                        </Switch>
                    </PageFade>
                </TransitionGroup>
            </Layout>
        );
    }
}
