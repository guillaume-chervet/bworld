import app from '../../app.module';
import { service as elementForm } from './elementForm-factory.js';
import view from './form_admin.html';

const name = 'elementFormAdmin';

function ElementController() {
  var ctrl = this;

  ctrl.addTab = elementForm.addTab;

  ctrl.removeTab = function(element, child) {
    const index = element.childs.indexOf(child);
    if (index > -1) {
      element.childs.splice(index, 1);
    }
  };

  ctrl.getTitle = elementForm.getTitle;
  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    element: '=',
    mode: '<',
    onChange: '<',
  },
});
