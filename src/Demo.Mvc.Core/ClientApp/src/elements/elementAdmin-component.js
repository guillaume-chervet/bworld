import app from '../app.module';
import { service as elementService } from './element-factory';
import view from './admin.html';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';

const name = 'elementAdmin';

class Controller {
  $onInit() {
    var ctrl = this;
    elementService.inherit(ctrl);
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  transclude: {
    edit: 'adminEdit',
    view: 'adminView',
    title: 'adminTitle',
    menu: '?adminMenu',
    add: '?adminAdd',
  },
  bindings: {
    element: '=',
    mode: '<',
  },
});


export const ElementAdmin = ({adminEdit, adminView, adminTitle, adminMenu, adminAdd}) => {

  return (
      <div className="mw-edit-panel" ng-mouseenter="$ctrl.select()" ng-mouseleave="$ctrl.unselect()">

        <div data-ng-class="{'hover': $ctrl.isSelect(), 'open': $ctrl.isEditMode()}">
          <button ng-show="$ctrl.isEditButton()" className="btn btn-success mw-edit" ng-click="$ctrl.clickEdit()"
                  type="button" data-toggle="dropdown" aria-expanded="true">
            <span className="glyphicon glyphicon glyphicon-edit"></span>
          </button>
          <div ng-if="$ctrl.isEditMode()">

            <div ng-form name="fieldForm" className="form-group">
              <label htmlFor="{{$ctrl.element.property}}" className="col-sm-6 col-md-6 col-xs-4 control-label"
                     ng-transclude="title">Title</label>
              <div ng-transclude="menu">
                <element-menu element="$ctrl.element" className="col-sm-6 col-md-6 col-xs-8">
                </element-menu>
              </div>
              <div className="col-sm-12 col-md-12 col-xs-12">
                <div ng-transclude="edit"></div>
              </div>
            </div>
          </div>
          <div ng-if="!$ctrl.isEditMode()" ng-transclude="view"></div>
        </div>
        <div ng-transclude="add">
          <element-menu-item ng-if="$ctrl.isLastElement() || $ctrl.isEditMode()" element="$ctrl.element"
                             mode="$ctrl.mode">
          </element-menu-item>
        </div>
      </div>
  );
};

export default name;