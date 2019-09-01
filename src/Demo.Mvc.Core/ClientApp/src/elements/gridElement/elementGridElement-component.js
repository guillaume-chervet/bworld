import app from '../../app.module';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import { service as elementGridElementService } from './elementGridElement-factory';
import { Div } from '../div/elementDiv-component';

const Child = ({ element }) => {
    if (!element) {
        return null;
    }
    const type = !element.data ? '1' : element.data.type; 

  switch (type) {
    case "2":
      return (<div className="panel panel-default">
        {element.data.title && <div className="panel-heading" >
          <h2 className="panel-title" >{element.data.title}</h2>
        </div>}
        <div className="panel-body">
          <Div element={element}>
          </Div>
        </div>
        {element.data.footer && <div className="panel-footer">
          <p>{element.data.footer}</p>
        </div>}
      </div>);
    default:
      return (<Div element={element} />);
  }
};

export const GridElement = ({element, noClass}) => {
  const cssClass = `mw-grid-element ${elementGridElementService.cssClass(element, noClass)}`;
  return (
      <div className={cssClass}>
        <Child element={element} />
      </div>
  );
};

const name = 'elementGridElement';
app.component(name, react2angular(GridElement, ['element', 'noClass']));

export default name;
