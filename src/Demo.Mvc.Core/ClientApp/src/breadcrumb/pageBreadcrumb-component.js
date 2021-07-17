import app from '../app.module';
import view from './pageBreadcrumb.html';
import redux from '../redux';
import React from 'react';
import { Breadcrumb } from './breadcrumb-component';
import { withStore } from '../reducers.config';
import { connect } from 'react-redux';

const name = 'pageBreadcrumb';

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    const ctrl = this;
    return ctrl;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { master: state.master };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: Controller,
  transclude: {
    content: 'content',
  },
});

const PageBreadcrumb = ({ children, master }) => {
  return (
    <div className="row">
      <div className="col-md-10 col-md-offset-1 col-lg-8 col-lg-offset-2 col-xs-12 col-xs-offset-0">
        <Breadcrumb master={master}/>
        {children}
        <Breadcrumb master={master}/>
      </div>
    </div>
  );
};

const mapStateToProps = (state, ownProps) => {
  return { master: state.master };
};

const mapDispatchToProps = (dispatch, ownProps) => {
  return {};
};

export const PageBreadcrumbWithState = withStore(
  connect(
    mapStateToProps,
    mapDispatchToProps
  )(PageBreadcrumb)
);

export default name;
