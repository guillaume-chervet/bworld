import { addElement } from '../free/add/addElement-factory';
import React from 'react';

export const ElementMenuItem = ({ element, mode, onChange }) => {
  const open = () => {
    addElement.openAsync(element, mode).result.then((selectedItem) => {
      onChange({ what: "element-add", element, selectedItem});
    });
  };
  return (
    <div className="mw-add text-center">
      <button type="button" className="btn btn-primary btn-lg" onClick={open}>
        <span className="glyphicon glyphicon-plus"/>
        <span>Ajouter</span>
      </button>
    </div>
  );
};
