import app from '../../app.module';
import view from './h2.html';

const name = 'elementH2';

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
  },
});

export default name;
