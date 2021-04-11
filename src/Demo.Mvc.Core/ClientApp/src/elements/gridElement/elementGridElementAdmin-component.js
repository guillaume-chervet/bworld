import app from '../../app.module';
import { service as elementGridElementService } from './elementGridElement-factory';
import { service as elementService } from '../element-factory';
import view from './gridElement_admin.html';
import {ElementAdmin} from "../elementAdmin-component";
import {LoadableRteEditor} from "../p/TextEditor";
import {Paragraphe} from "../p/elementP-component";
import {react2angular} from "react2angular";
import React from "react";

const name = 'elementGridElementAdmin';

function ElementController() {
  var ctrl = this;
  elementService.inherit(ctrl);

  ctrl.cssClass = elementGridElementService.cssClass;
  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;

/*
const Admin = () => {


 return (<div className="row mw-grid-element">
    <div ng-form name="fieldForm" className="form-group">
      <label htmlFor="Type" className="col-sm-12 col-md-12 col-xs-12 control-label">Type affichage</label>
      <div className="col-sm-12 col-md-12 col-xs-12">
        <select id="Type" name="uType" className="form-control" ng-model="$ctrl.element.data.type">
          <option value="">- Sélectionner -</option>
          <option value="1">Colonne</option>
          <option value="2">Panel</option>
        </select>
      </div>
    </div>

    <div className="animate-switch-container"
         ng-switch on="$ctrl.element.data.type">
      <div className="animate-switch" ng-switch-when="2">
        <div className="panel panel-default">
          <div className="panel-heading">
            <div ng-form name="fieldForm" className="form-group">
              <label htmlFor="title" className="col-sm-12 col-md-12 col-xs-12 control-label">Titre en-tête</label>
              <div className="col-sm-12 col-md-12 col-xs-12">
                <input type="text" name="field" id="title" ng-model="$ctrl.element.data.title"
                       className="form-control"/>
              </div>
            </div>
          </div>
          <div className="panel-body">
            <element-div-admin element="$ctrl.element">
            </element-div-admin>
          </div>
          <div className="panel-footer">
            <div ng-form name="fieldForm" className="form-group">
              <label htmlFor="footer" className="col-sm-12 col-md-12 col-xs-12 control-label">Titre pied</label>
              <div className="col-sm-12 col-md-12 col-xs-12">
                <input type="text" name="field" id="footer" ng-model="$ctrl.element.data.footer"
                       className="form-control"/>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="animate-switch" ng-switch-default>
        <element-div-admin element="$ctrl.element">
        </element-div-admin>
      </div>
    </div>
  </div>);
}

export const ElementGridElementAdmin = ({ element, mode, onChange }) => {
  const data = clear_attr(element.data, []);
  const onChangeWrapper = (html) => {
    onChange({ what: "element-edit", element: {...element, data:html}});
  };
  
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Texte'}
          adminEdit={<LoadableRteEditor
              value={data}
              onChange={onChangeWrapper}

          />}
          adminView={<Paragraphe element={element} />}>
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementGridElementAdmin, ['element', 'mode', 'onChange']));

export default name;
*/