import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { module } from './module-factory';
import view from './modulesAdmin.html';

const name = 'modulesAdmin';

class Controller {
  $onInit() {
    page.setTitle('Visualiser les modules', page.types.superAdmin);

    const vm = this;
    vm.search = {
      moduleId: '',
    };

    vm.module = {
      item: '',
      json: '',
    };

    vm.get = function() {
      if (vm.search.moduleId) {
        module.superAdminGetAsync(vm.search).then(function(data) {
          if (data && data.data) {
            const item = data.data;
            vm.module.json = JSON.stringify(JSON.parse(item.json), null, '\t');
            delete item.json;
            vm.module.item = JSON.stringify(JSON.parse(item), null, '\t');
          }
        });
      }
    };

    vm.save = function() {
      if (vm.module.item) {
        const item = {};
        Object.assign(item, JSON.parse(vm.module.item));
        item.json = vm.module.json;
        module.superAdminSaveAsync(item);
      }
    };

    vm.delete = function() {
      if (vm.module.item && vm.module.item.id) {
        module.superAdminDeleteAsync(vm.module.item.id).then(function() {
          vm.module.item = null;
          vm.module.json = null;
        });
      }
    };

    return vm;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
