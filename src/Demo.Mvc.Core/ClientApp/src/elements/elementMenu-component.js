import app from '../app.module';
import { menu as elementMenuService } from './elementMenu-factory';
import React from 'react';
import { react2angular } from 'react2angular';

const name = 'elementMenu';

export const ElementMenu = ({element}) => {

  const parent = element.$parent;
  const deleteElement = () => {
    elementMenuService.deleteElement(element, parent);
  };

  const up = () =>  elementMenuService.up(element, parent);
  const down = () => elementMenuService.down(element, parent);
  const canUp = () => elementMenuService.canUp(element, parent);

  const canDown = () => elementMenuService.canDown(element, parent);
  
  return (
      <div className="btn-toolbar mw-toolbar" role="toolbar" aria-label="Toolbar with button groups">
        <div className="btn-group pull-right" role="group" aria-label="First group">
          {canUp() && (<button type="button" onClick={up} 
                  className="btn btn-primary"><span className="glyphicon glyphicon-chevron-up"
                                                    aria-hidden="true"></span></button>)}
          {canDown() && (<button type="button" onClick={down} 
                  className="btn btn-primary"><span className="glyphicon glyphicon-chevron-down"
                                                    aria-hidden="true"></span></button>)}
        </div>
        <div className="btn-group pull-right" role="group" aria-label="Second group">
          <button type="button" onClick={deleteElement} className="btn btn-danger"><span
              className="glyphicon glyphicon-trash" aria-hidden="true"></span></button>
        </div>
      </div>
  );
};

app.component(name, react2angular(ElementMenu, ['element']));

export default name;
