import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { users } from './users-factory';
import view from './usersAdmin.html';

const name = 'usersAdmin';

class Controller {
  $onInit() {
    page.setTitle('Liste utilisateurs', page.types.superAdmin);
    const vm = this;
    vm.users = users.users;
    vm.delete = function(user) {
      users.deleteAsync(user);
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
