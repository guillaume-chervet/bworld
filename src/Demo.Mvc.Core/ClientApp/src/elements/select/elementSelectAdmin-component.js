import app from '../../app.module';
import view from './select_admin.html';

const name = 'elementSelectAdmin';

class Controller {
  $onInit() {
    var ctrl = this;
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
