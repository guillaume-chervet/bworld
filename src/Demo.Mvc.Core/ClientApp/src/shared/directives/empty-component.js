import app from '../../app.module';
import React from 'react';
import { react2angular } from 'react2angular';

const name = 'empty';

export const Empty = ({ items, content }) => {
  if(!items) { 
    return null
  }
  if(items.length>0) {
    return null;
  }
  return (
      <div className="mw-empty"><p>{content}</p></div>
  );
};

app.component(name, react2angular(Empty, ['items', 'content']));

export default name;
