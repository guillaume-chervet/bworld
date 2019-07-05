import app from '../../app.module';
import view from './subMenuItemsAdmin.html';

const name = 'subMenuItemsAdmin';

class Controller {
  $onInit() {
    const vm = this;
    return vm;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    menuItems: '=',
  },
});

export default name;
