import app from '../../app.module';
import { free } from '../../free/free-factory';
import { page } from '../../shared/services/page-factory';
import { site } from './site-factory';
import view from './siteAdmin.html';

const name = 'siteAdmin';

class Controller {
  $onInit() {
    const vm = this;
    page.setTitle('Site', page.types.admin);
    const parentsJson = {
      type: 'div',
      childs: site.elements,
    };
    vm.element = free.mapParent(parentsJson);
    vm.submit = function(form) {
      if (form.$valid) {
        site.saveAsync();
      }
    };
    return vm;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
