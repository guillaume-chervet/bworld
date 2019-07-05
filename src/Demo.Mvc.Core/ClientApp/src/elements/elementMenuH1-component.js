import app from '../app.module';
import { menu as elementMenuService } from './elementMenu-factory';
import view from './menu-h1.html';

const name = 'elementMenuH1';

class Controller {
  $onInit() {
    var ctrl = this;

    var parent = ctrl.element.$parent;

    ctrl.up = elementChild => {
      elementMenuService.up(elementChild, parent);
    };

    ctrl.down = elementChild => {
      elementMenuService.down(elementChild, parent);
    };

    ctrl.canUp = elementChild => {
      return elementMenuService.canUp(elementChild, parent);
    };

    ctrl.canDown = elementChild => {
      return elementMenuService.canDown(elementChild, parent);
    };
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
  },
});

export default name;
