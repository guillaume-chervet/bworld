import app from '../../app.module';
import view from './color_admin.html';

const name = 'elementColorAdmin';

class Controller {
  $onInit() {
    const ctrl = this;
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: Controller,
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;
