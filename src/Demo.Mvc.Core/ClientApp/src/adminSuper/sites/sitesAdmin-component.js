import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { sites } from './sites-factory';
import view from './sitesAdmin.html';

const name = 'sitesAdmin';

class Controller {
  $onInit() {
    page.setTitle('Liste sites', page.types.superAdmin);
    const vm = this;
    vm.sites = sites.sites;

    vm.delete = function(site) {
      sites.deleteAsync(site);
    };

    vm.clearCache = function(site) {
      sites.clearCacheAsync(site);
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
