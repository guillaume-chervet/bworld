import app from '../../app.module';
import view from './div.html';

const name = 'elementDiv';

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
