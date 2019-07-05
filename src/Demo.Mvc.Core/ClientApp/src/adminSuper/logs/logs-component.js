import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { logs } from './logs-factory';
import view from './logs.html';

const name = 'logs';

class Controller {
  $onInit() {
    page.setTitle('Logs', page.types.superAdmin);
    const vm = this;
    vm.data = logs.data;

    // TODO
    vm.filter = {
      beginDate: '',
      endDate: '',
    };

    if (!vm.filter.endDate) {
      vm.filter.endDate = new Date().toUTCString();
    }

    if (!vm.filter.beginDate) {
      const now = new Date();
      now.setMinutes(now.getMinutes() - 160);
      vm.filter.beginDate = now.toUTCString();
    }

    vm.clear = function() {
      logs.clearAsync();
    };

    vm.submit = function() {
      logs.initAsync(vm.filter);
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
