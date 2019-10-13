import app from '../app.module';
import history from '../history';
import { page } from '../shared/services/page-factory';
import view from './addSiteConfirm.html';

const name = 'addSiteConfirm';

class Controller {
  $onInit() {
    const vm = this;
    page.setTitle('Création site confirmation');
    vm.returnUrl = '/administration';
    vm.goAdministration = function() {
      history.search({'dm': null}, '/administration');
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
