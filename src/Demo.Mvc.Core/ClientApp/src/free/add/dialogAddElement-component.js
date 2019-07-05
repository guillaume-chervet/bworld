import app from '../../app.module';
import view from './dialogAddElement.html';

const name = 'dialogAddElement';

class Controller {
  $onInit() {
    const ctrl = this;
    ctrl.item = ctrl.resolve.item;
    ctrl.model = {};
    ctrl.ok = function() {
      ctrl.close({ $value: ctrl.model.selected });
    };
    ctrl.cancel = function() {
      ctrl.dismiss();
    };
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    resolve: '<',
    close: '&',
    dismiss: '&',
  },
});

export default name;
