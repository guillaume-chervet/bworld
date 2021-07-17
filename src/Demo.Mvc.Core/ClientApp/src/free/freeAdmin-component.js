import app from '../app.module';
import { page } from '../shared/services/page-factory';
import { free } from './free-factory';
import { freeInit, freeOnChange } from './free-actions';
import { master } from '../shared/providers/master-provider';
import view from './free_admin.html';

import redux from '../redux';
import {react2angular} from "react2angular/index";
import React, {useEffect, useMemo} from "react";
import {PageBreadcrumbWithState} from "../breadcrumb/pageBreadcrumb-component";
import {DivAdmin} from "../elements/div/elementDivAdmin-component";
import {withStore} from "../reducers.config";
import {connect} from "react-redux";

const name = 'freeAdmin';
/*
class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    const vm = this;
    const title = free.getTitle(free.data.elements);
    page.setTitle(title, page.types.admin);

    vm.element = free.mapParent({ type: 'div', childs: free.elements });

    vm.metaElement = free.mapParent({
      type: 'div',
      childs: free.data.metaElements,
    });

    vm.data = free.data.data;

    vm.icons = [
      'fa fa-file-o',
      'glyphicon glyphicon-envelope',
      'fa fa-home',
      'fa fa-address-book',
      'fa fa-bicycle',
      'fa fa-shower',
      'fa fa-bath',
      'fa fa-coffee',
      'fa fa-history',
      'fa fa-coffee',
      'fa fa-shopping-cart',
      'fa fa-shield',
      'fa fa-book',
      'fa-address-book',
      'fa-car',
      'fa-exclamation',
      'fa-diamond',
      'fa-child',
      'fa-flash',
    ];

    const moduleId = master.getModuleId();
    vm.submit = () => {
      if (free.isUploading(vm.element.childs)) {
        return;
      }
      free.saveAsync(moduleId);
    };

    vm.delete = () => {
      if (free.isUploading(vm.element.childs)) {
        return;
      }
      module.deleteAsync(moduleId);
    };

    vm.isButtonDisabled = () => {
      return free.isUploading(vm.element.childs);
    };

    const dispatch = redux.getDispatch();
    dispatch(freeInit(vm));
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { free: state.free };
  }
  mapThisToProps() {
    return {
      onChange: (e) => redux.getDispatch()(freeOnChange(e)),
    };
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});
*/

const FreeAdmin = ({onChange}) => {
  useEffect(() => {
    const title = free.getTitle(free.data.elements);
    page.setTitle(title, page.types.admin);
  });

  const memData = useMemo(() => {
    const vm = {};
    vm.element = free.mapParent({ type: 'div', childs: free.elements });

    vm.metaElement = free.mapParent({
      type: 'div',
      childs: free.data.metaElements,
    });

    vm.data = free.data.data;

    vm.icons = [
      'fa fa-file-o',
      'glyphicon glyphicon-envelope',
      'fa fa-home',
      'fa fa-address-book',
      'fa fa-bicycle',
      'fa fa-shower',
      'fa fa-bath',
      'fa fa-coffee',
      'fa fa-history',
      'fa fa-coffee',
      'fa fa-shopping-cart',
      'fa fa-shield',
      'fa fa-book',
      'fa-address-book',
      'fa-car',
      'fa-exclamation',
      'fa-diamond',
      'fa-child',
      'fa-flash',
    ];

    const moduleId = master.getModuleId();
    vm.submit = (e) => {
      e.preventDefault()
      if (free.isUploading(vm.element.childs)) {
        return;
      }
      free.saveAsync(moduleId);
    };

    vm.delete = (e) => {
      e.preventDefault()
      if (free.isUploading(vm.element.childs)) {
        return;
      }
      module.deleteAsync(moduleId);
    };

    vm.isButtonDisabled = () => {
      return free.isUploading(vm.element.childs);
    };
    const dispatch = redux.getDispatch();
    dispatch(freeInit(vm));
    return vm;
    
  }, []);

  return (
      <PageBreadcrumbWithState>
        <h1><span className="fa fa-file-o"/> Page libre</h1>
        <p>Cliquer sur les éléments ci-dessous afin de pouvoir les éditer.</p>
        <div className="mw-free">
          <form name="freeForm" role="form" className="form-horizontal" encType="multipart/form-data" onSubmit={memData.submit}>
            <h2>Contenu</h2>
                <DivAdmin className="mw-free" element={memData.element} onChange={onChange}/>
            <hr />
            <div class="form-group mw-action-bar">
              <div class="col-sm-3 col-xs-6">
              </div>
              <div class="col-sm-9 col-xs-6 mw-action">
                <button type="submit" class="btn btn-lg btn-success"><span className="glyphicon glyphicon-floppy-disk"/> Sauvegarder</button>
              </div>
            </div>
          </form>
        </div>
      </PageBreadcrumbWithState>
  );
};


const mapStateToProps = (state, ownProps) => {
  return { free: state.free };
};

const mapDispatchToProps = (dispatch, ownProps) => {
  return {
    onChange: (e) => redux.getDispatch()(freeOnChange(e)),
  };
};

export const FreeAdminWithState = withStore(
    connect(
        mapStateToProps,
        mapDispatchToProps
    )(FreeAdmin)
);

app.component(name, react2angular(FreeAdminWithState, []));

export default name;
