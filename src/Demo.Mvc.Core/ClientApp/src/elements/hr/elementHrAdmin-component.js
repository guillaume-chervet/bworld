import app from '../../app.module';
import view from './hr_admin.html';

const name = 'elementHrAdmin';

class Controller {
  $onInit() {
    const ctrl = this;
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
