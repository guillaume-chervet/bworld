import app from '../../app.module';
import React from 'react';
import { react2angular } from 'react2angular';

const name = 'empty';

export const Empty = ({ items, text }) => {
  if( items || items.length>0) { 
    return null
  }
  return (
      <div className="mw-empty"><p>{text}</p></div>
  );
};

app.component(name, react2angular(Empty, ['items', 'text']));

export default name;
