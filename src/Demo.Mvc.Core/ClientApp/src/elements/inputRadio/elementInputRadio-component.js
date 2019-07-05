import app from '../../app.module';
import { service as elementForm } from '../form/elementForm-factory.js';
import view from './inputRadio.html';

const name = 'elementInputRadio';

function ElementController() {
  const ctrl = this;

  ctrl.model = elementForm.getUserData(ctrl.element);

  ctrl.disabled = function() {
    return (
      ctrl.model.responseSubmited && ctrl.model.isValid && ctrl.model.showResult
    );
  };

  ctrl.showError = function() {
    return ctrl.model.isValid === false && ctrl.model.showResult;
  };

  ctrl.showSuccess = function() {
    return ctrl.model.isValid === true && ctrl.model.showResult;
  };

  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    element: '=',
  },
});

export default name;
