import app from '../../app.module';
import { guid } from '../../shared/services/guid-factory';
import view from './inputRadio_admin.html';

const name = 'elementInputRadioAdmin';

function ElementController() {
  const ctrl = this;

  ctrl.addOption = function() {
    ctrl.element.data.options.push({
      id: guid.guid(),
      label: 'Nouvelle réponse possible',
    });
  };

  ctrl.removeOption = function(option) {
    var index = ctrl.element.data.options.indexOf(option);
    if (index > -1) {
      ctrl.element.data.options.splice(index, 1);
    }
  };

  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;
