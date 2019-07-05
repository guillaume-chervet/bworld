import app from '../app.module';
import { page } from '../shared/services/page-factory';
import { addSite } from './addSite-factory';
import view from './addSiteAdmin.html';

const name = 'addSiteAdmin';

class Controller {
  $onInit() {
    const vm = this;
    page.setTitle('Page création site');
    vm.data = addSite.data;
    vm.add = function() {
      const element = {
        siteId: '',
        categoryId: '',
        workName: '',
        title: '',
      };
      vm.data.templates.push(element);
    };
    vm.delete = function(element) {
      const childs = vm.data.templates;
      while (childs.indexOf(element) !== -1) {
        childs.splice(childs.indexOf(element), 1);
      }
    };
    vm.submit = function() {
      addSite.saveAdminAsync(vm.data);
    };
    vm.navBack = addSite.navBack;
    const parentsJson = addSite.mapParent({
      type: 'div',
      childs: addSite.pageData.elements,
    });
    vm.element = parentsJson;
    const metaParentsJson = addSite.mapParent({
      type: 'div',
      childs: addSite.pageData.metaElements,
    });
    vm.metaElement = metaParentsJson;
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
