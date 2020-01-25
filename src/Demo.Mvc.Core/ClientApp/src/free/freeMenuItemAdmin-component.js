import app from '../app.module';
import { isDeleted, isDraft } from '../shared/itemStates';
import { getIcon } from '../shared/icons';
import view from './freeMenuItemAdmin.html';

const name = 'freeMenuItemAdmin';

class Controller {
  $onInit() {
    const vm = this;
    vm.isDeleted = isDeleted;
    vm.isDraft = isDraft;
    vm.getIcon = getIcon;
    return vm;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    menuItem: '=',
  },
});

export default name;
