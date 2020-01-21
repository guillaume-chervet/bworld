import app from '../../app.module';
import view from './checkbox_admin.html';

const name = 'elementCheckboxAdmin';

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
