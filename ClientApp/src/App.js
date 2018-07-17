import React, { Component } from 'react';
import { Route } from 'react-router';
import { Switch, Redirect } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Auth, Home, Summary, Details, Config, NotFound, Report } from './components/MenuList';
import { ToastContainer } from 'react-toastify';
import ErrorBoundary from './plugin/error/ErrorBoundary';
//import { CSSTransition, TransitionGroup } from 'react-transition-group';

//const PageFade = (props) => (
//    <CSSTransition
//        {...props}
//        classNames="fadeTranslate"
//        timeout={500}
//        mountOnEnter={true}
//        unmountOnExit={true}
//    />
//)

const MainRoute = ({ component: Component, ...rest }) => (
    localStorage.getItem("token") !== null ?
        <Route {...rest} render={props => (
            <Layout>
                <Component {...props} />
            </Layout>
        )} />
        :
        <Redirect to="/" />
)

MainRoute.displayName = "MainRoute";

export default class App extends Component {    
    displayName = "App";
    render() {
        return (
            <div>
                <ErrorBoundary>
                    <Switch location={this.props.location}>
                        <Route exact path='/' component={Auth} />
                        <MainRoute exact path='/home' component={Home} />
                        <MainRoute exact path='/summary' component={Summary} />
                        <MainRoute exact path='/data' component={Details} />
                        <MainRoute exact path='/shipping' component={Details} />
                        <MainRoute exact path='/report' component={Report} />
                        <MainRoute exact path='/report/:id' component={Report} />
                        <MainRoute exact path='/config' component={Config} />
                        <MainRoute exact path='/config/:id' component={Config} />
                        <MainRoute exact component={NotFound} />
                    </Switch>
                    <ToastContainer
                        position="top-left"
                        autoClose={10000}
                        hideProgressBar={false}
                        newestOnTop
                        closeOnClick
                        rtl={false}
                        pauseOnVisibilityChange
                        draggable
                        pauseOnHover
                    />
                </ErrorBoundary>
            </div>
        );
    }
}

//<div>
//    <Layout location={locationKey}>
//        <TransitionGroup>
//            <PageFade key={locationKey}>
//                <Switch location={this.props.location}>
//                    <Route exact path='/' component={Auth} />
//                    <Route exact path='/home' component={Home} />
//                    <Route exact path='/data' component={Sample} />
//                    <Route exact path='/report' component={Report} />
//                    <Route exact path='/external' component={External} />
//                    <Route exact path='/config' component={Config} />
//                    <Route exact component={NotFound} />
//                </Switch>
//            </PageFade>
//        </TransitionGroup>
//    </Layout>
//</div>