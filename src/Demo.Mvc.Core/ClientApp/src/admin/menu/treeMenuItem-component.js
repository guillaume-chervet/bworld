import app from '../../app.module';
import { isDeleted, isDraft } from '../../shared/itemStates';
import view from './treeMenuItem.html';

const name = 'treeMenuItem';

class Controller {
  $onInit() {
    const vm = this;
    vm.isDraft = isDraft;
    vm.isDeleted = isDeleted;
    vm.getClass = function(menuItem) {
      if (!menuItem) {
        return '';
      }
      if (isDraft(menuItem)) {
        return 'mw-draft';
      }
      if (isDeleted(menuItem)) {
        return 'mw-deleted';
      }
      return '';
    };
    return this;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  transclude: {
    actions: 'treeMenuActions',
  },
  bindings: {
    canUp: '<',
    canDown: '<',
    up: '<',
    down: '<',
    slideMenu: '<',
    setChild: '<',
    canSetChild: '<',
    setParent: '<',
    canSetParent: '<',
    menuItems: '=',
    title: '<',
  },
});

export default name;
