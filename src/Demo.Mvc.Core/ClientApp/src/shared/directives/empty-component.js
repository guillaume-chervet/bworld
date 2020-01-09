﻿import app from '../../app.module';
import React from 'react';
import { react2angular } from 'react2angular';

const name = 'empty';

export const Empty = ({ items, text }) => {
  if( items || (items && items.length>0)) { 
    return null
  }
  const newText = text || "Aucun élément dans la liste";
  return (
      <div className="mw-empty"><p>{newText}</p></div>
  );
};

app.component(name, react2angular(Empty, ['items', 'text']));

export default name;
