import app from '../../app.module';
import React from 'react';
import { react2angular } from 'react2angular';
import { GridElement } from '../gridElement/elementGridElement-component';

export const Grid = ({ element }) => {
  return (
    <div className="row mw-grid">
      {element.childs.map((childElement, $index) => (
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
    </div>
  );
};

const name = 'elementGrid';
app.component(name, react2angular(Grid, ['element']));

export default name;
