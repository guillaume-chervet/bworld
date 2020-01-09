import app from '../app.module';
import { menu as elementMenuService } from './elementMenu-factory';
import React from 'react';
import { react2angular } from 'react2angular';

export const ElementMenuH1= ({element}) => {
  const parent = element.$parent;

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
      </div>
  );
};

const name = 'elementMenuH1';
app.component(name, react2angular(ElementMenuH1, ['element']));

export default name;

