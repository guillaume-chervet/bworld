import app from '../app.module';
import { menu as elementMenuService } from './elementMenu-factory';
import view from './menu.html';

const name = 'elementMenu';

class Controller {
  $onInit() {
    const ctrl = this;

    const parent = ctrl.element.$parent;
    ctrl.deleteElement = elementChild => {
      elementMenuService.deleteElement(elementChild, parent);
    };

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
