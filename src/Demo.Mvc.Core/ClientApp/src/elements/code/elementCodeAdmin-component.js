import app from '../../app.module';
import './elementCodePanel-component';
import './code.css';
import view from './code_admin.html';

const name = 'elementCodeAdmin';

class Controller {
  $onInit() {
    const ctrl = this;
    ctrl.onChange = newElement => {
      ctrl.element.data.files = newElement.data.files;
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
