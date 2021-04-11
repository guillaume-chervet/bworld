import app from '../../app.module';
import { service as elementService } from '../element-factory';
import view from './grid_admin.html';
import {GridElement} from "../gridElement/elementGridElement-component";
import {react2angular} from "react2angular";
import React from "react";
import {ElementAdmin} from "../elementAdmin-component";
import {H1} from "../h1/elementH1-component";
import {ElementMenuH1} from "../elementMenuH1-component";
import {Grid} from "./elementGrid-component";

const name = 'elementGridAdmin';

function ElementController() {
  const ctrl = this;
  elementService.inherit(ctrl);
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
const name = 'elementGridAdmin';

const GridEdit = ({element, onChange}) => {
  return <>  {element.childs.map((childElement, $index) => (
      <>
        <GridElement element={childElement} />
        {($index + 1) % 2 === 0 && (
            <div className="clearfix visible-sm-block"></div>
        )}
        {($index + 1) % 3 === 0 && (
            <div className="clearfix visible-md-block"></div>
        )}
        {($index + 1) % 4 === 0 && (
            <div className="clearfix visible-lg-block"></div>
        )}
      </>
  ))}
  <div ng-repeat="element in $ctrl.element.childs">
    <div ng-switch on="element.type">
      <element-grid-element-admin className="animate-switch" ng-switch-when="gridElement" element="element"
                                  on-change="$ctrl.onChange">
      </element-grid-element-admin>
      <div className="animate-switch" ng-switch-default>default</div>
    </div>
    <div ng-if="($index+1)%1 ===0" className="clearfix visible-xs-block"></div>
    <div ng-if="($index+1)%2 ===0" className="clearfix visible-sm-block"></div>
    <div ng-if="($index+1)%3 ===0" className="clearfix visible-md-block"></div>
    <div ng-if="($index+1)%4 ===0" className="clearfix visible-lg-block"></div>
  </div>
  <div className="clearfix"/>)</>;
  
  return (<input className="form-control" type="text" name="field" id={element.property} value={element.data} onChange={onChange} />);
};

export const ElementGridAdmin = ({ element, mode, onChange }) => {
  const onChangeWrapper = (e) => {
    onChange({ what: "element-edit", element: {...element, data: e.target.value}});
  };
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Grille'}
          adminEdit={<GridEdit element={element} onChange={onChangeWrapper} />}
          adminView={<Grid element={element} />}
      >
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementGridAdmin, ['element', 'mode', 'onChange']));

export default name;

*/
