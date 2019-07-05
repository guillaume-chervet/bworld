import app from '../../app.module';
import { service as elementGridElementService } from './elementGridElement-factory';
import { service as elementService } from '../element-factory';
import view from './gridElement_admin.html';

const name = 'elementGridElementAdmin';

function ElementController() {
  var ctrl = this;
  elementService.inherit(ctrl);

  ctrl.cssClass = elementGridElementService.cssClass;
  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;
