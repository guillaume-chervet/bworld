import app from '../../app.module';
import { service as elementService } from '../element-factory';
import view from './h1_admin.html';

const name = 'elementH1Admin';

function ElementController() {
  var ctrl = this;
  elementService.inherit(ctrl);
  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    element: '=',
    mode: '<',
    onChange: '<',
  },
});

export default name;
