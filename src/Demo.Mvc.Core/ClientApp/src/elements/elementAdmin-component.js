import app from '../app.module';
import { service as elementService } from './element-factory';
import view from './admin.html';

const name = 'elementAdmin';

class Controller {
  $onInit() {
    var ctrl = this;
    elementService.inherit(ctrl);
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  transclude: {
    edit: 'adminEdit',
    view: 'adminView',
    title: 'adminTitle',
    menu: '?adminMenu',
    add: '?adminAdd',
  },
  bindings: {
    element: '=',
    mode: '<',
  },
});

export default name;
