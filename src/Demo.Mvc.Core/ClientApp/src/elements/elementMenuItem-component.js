import app from '../app.module';
import { addElement } from '../free/add/addElement-factory';
import { service as elementService } from './element-factory';
import React from 'react';
import { react2angular } from 'react2angular';

export const ElementMenuItem = ({element, mode}) => {
  const open = () => {
    addElement.openAsync(element, mode).result.then(function(selectedItem) {
      elementService.addElement(selectedItem, element);
    });
  };
  return (
      <div className="mw-add text-center">
        <button type="button" className="btn btn-primary btn-lg" onClick={open}>
          <span className="glyphicon glyphicon-plus"></span>
          <span>Ajouter</span>
        </button>
      </div>
  );
};

const name = 'elementMenuItem'; 
app.component(name, react2angular(ElementMenuItem, ['element', 'mode']));

export default name;
