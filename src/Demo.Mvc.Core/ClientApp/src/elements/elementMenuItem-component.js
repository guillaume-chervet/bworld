import app from '../app.module';
import { addElement } from '../free/add/addElement-factory';
import { service as elementService } from './element-factory';
import view from './menu-item.html';

const name = 'elementMenuItem';
class Controller {
  $onInit() {
    var ctrl = this;

    ctrl.open = function(elem, mode) {
      addElement.openAsync(elem, mode).result.then(function(selectedItem) {
        elementService.addElement(selectedItem, elem);
      });
    };

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    mode: '<',
  },
});

export default name;
