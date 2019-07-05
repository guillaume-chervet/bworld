import app from '../app.module';
import { page } from '../shared/services/page-factory';
import { addSite } from './addSite-factory';
import { free } from '../free/free-factory';
import view from './addSite.html';

const name = 'addSite';

class Controller {
  $onInit() {
    const vm = this;
    const title = free.getTitle(addSite.pageData.elements);
    page.setTitle(title);

    vm.navStart = function() {
      addSite.navStart();
    };
    const parentsJson = addSite.mapParent({
      type: 'div',
      childs: addSite.pageData.elements,
    });
    vm.element = parentsJson;
    return vm;
  }
}

app.component(name, {
  template: view ,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
