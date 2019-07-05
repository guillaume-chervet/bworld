import app from '../../app.module';
import view from './table.html';

const name = 'elementTable';
class Controller {
  $onInit() {
    var ctrl = this;
    return ctrl;
  }
}
app.component(name, {
  template: view,
  controller: Controller,
  bindings: {
    element: '=',
    //  hero: '<',
    //onDelete: '&',
    // onUpdate: '&'
  },
});

export default name;
