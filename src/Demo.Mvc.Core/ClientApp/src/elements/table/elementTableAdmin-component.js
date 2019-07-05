import app from '../../app.module';
import view from './table_admin.html';

const name = 'elementTableAmin';

class Controller {
  $onInit() {
    var ctrl = this;
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;
