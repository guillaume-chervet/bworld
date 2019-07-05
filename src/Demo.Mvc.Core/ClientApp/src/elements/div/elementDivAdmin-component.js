import app from '../../app.module';
import { service as elementService } from '../element-factory';
import view from './div_admin.html';

const name = 'elementDivAdmin';

class Controller {
  $onInit() {
    const ctrl = this;
    elementService.inherit(ctrl);
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    mode: '<',
    onChange: '<',
  },
});

export default name;
