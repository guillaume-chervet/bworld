import app from '../../app.module';
import view from './dialogInfo.html';
var name = 'dialogInfo';

class Controller {
  $onInit() {
    const ctrl = this;
    ctrl.item = ctrl.data;

    ctrl.ok = function() {
      ctrl.close(ctrl.item);
    };

    ctrl.cancel = function() {
      ctrl.dismiss('cancel');
    };

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    data: '<',
    close: '&',
    dismiss: '&',
  },
  transclude: {},
});

export default name;
