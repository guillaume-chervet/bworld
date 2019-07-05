import app from '../../app.module';
import view from './menuItemsAdmin.html';

const name = 'menuItemsAdmin';

class Controller {
  $onInit() {
    const vm = this;
    return vm;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {
    menuItems: '=',
  },
});

export default name;
