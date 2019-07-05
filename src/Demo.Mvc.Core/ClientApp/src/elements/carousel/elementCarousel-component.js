import app from '../../app.module';
import view from './elementCarousel.html';

const name = 'elementCarousel';

class Controller {
  $onInit() {
    const ctrl = this;
    ctrl.active = 0;
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
