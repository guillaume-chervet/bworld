import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { menuAdmin } from './menuAdmin-factory';
import view from './menuAdmin.html';

import './menuAdmin.css';

const name = 'menuAdmin';

class Controller {
  $onInit() {
    page.setTitle('Menus', page.types.admin);
    menuAdmin.init();
    const vm = this;
    vm.menu = menuAdmin.menu;

    vm.canUp = menuAdmin.canUp;
    vm.canDown = menuAdmin.canDown;
    vm.up = menuAdmin.up;
    vm.down = menuAdmin.down;
    vm.slideMenu = menuAdmin.slideMenu;
    vm.setChild = menuAdmin.setChild;
    vm.canSetChild = menuAdmin.canSetChild;
    vm.setParent = menuAdmin.setParent;
    vm.canSetParent = menuAdmin.canSetParent;

    vm.submit = menuAdmin.saveAsync;

    return this;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
