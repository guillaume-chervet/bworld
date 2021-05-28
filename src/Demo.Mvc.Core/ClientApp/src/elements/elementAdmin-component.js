import { service as elementService, defaultState } from './element-factory';
import React, { useState } from 'react';
import classnames from 'classnames';
import { ElementMenu } from './elementMenu-component';
import { ElementMenuItem } from './elementMenuItem-component';
const name = 'elementAdmin';

export const ElementAdmin = ({
  adminEdit,
  adminView,
  adminTitle,
  adminMenu,
  adminAdd,
  element,
    onChange,
                               mode,
}) => {
  const [state , setState] = useState(defaultState);
  const $ctrl = elementService.inherit(undefined, element, state, setState);
  const className = classnames({'hover': $ctrl.isSelect(), 'open': $ctrl.isEditMode()});
  const styleButton = { visibility: $ctrl.isEditButton() ? 'visible': 'hidden' };
  return (
    <div
      className="mw-edit-panel"
      onMouseEnter={$ctrl.select}
      onMouseLeave={$ctrl.unselect}>
      <div className={className}>
        <button
          style={styleButton}
          className="btn btn-success mw-edit"
          onClick={$ctrl.clickEdit}
          type="button"
          data-toggle="dropdown"
          aria-expanded="true">
          <span className="glyphicon glyphicon glyphicon-edit"/>
        </button>
        {$ctrl.isEditMode() && <div>
          <div name="fieldForm" className="form-group">
            <label
              htmlFor={element.property}
              className="col-sm-6 col-md-6 col-xs-4 control-label"
              >
              {adminTitle}
            </label>
            { adminMenu ? <>{adminMenu}</> :
              <div className="col-sm-6 col-md-6 col-xs-8">
                <ElementMenu element={element} onChange={onChange} />
            </div>}
            <div className="col-sm-12 col-md-12 col-xs-12">
              {adminEdit && <>{adminEdit}</> }
            </div>
          </div>
        </div>}
        {!$ctrl.isEditMode() && <>{adminView}</> }
      </div>
      {adminAdd ? <>{adminAdd}</> :
          <>{$ctrl.isLastElement() || $ctrl.isEditMode() && <ElementMenuItem
    element={element}
    mode={mode} onChange={onChange}/> }</>
      }
    </div>
  );
};

export default name;
