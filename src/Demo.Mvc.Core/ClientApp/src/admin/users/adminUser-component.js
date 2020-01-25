import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import { in18Util } from '../../shared/services/in18Util-factory';
import { adminUser } from './adminUser-factory';
import { dialogTags } from '../tags/dialogTags-factory';
import redux from '../../redux';
import view from './adminUser.html';

import './adminUser.css';

const name = 'adminUser';

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    page.setTitle('Utilisateurs', page.types.admin);

    const vm = this;
    vm.data = adminUser.data;
    vm.getTagInfo = dialogTags.getTagInfo;
    vm.display = in18Util.display;

    vm.model = {
      mail: '',
    };

    vm.rules = {
      mail: ['required', 'email'],
    };

    const _order = function(predicate) {
      vm.Sort.reverse =
        vm.Sort.predicate === predicate ? !vm.Sort.reverse : false;
      vm.Sort.predicate = predicate;
    };

    vm.Sort = {
      predicate: 'Date',
      reverse: true,
      order: _order,
    };

    vm.remove = function(user) {
      if (user) {
        adminUser.removeAsync(user);
      }
    };

    vm.canRemove = function(u) {
      if (u.id === vm.userId) {
        return false;
      }
      return adminUser.data.users.length > 1;
    };

    vm.navAddUser = function() {
      history.path('/administration/utilisateurs/edition');
    };

    vm.navEditUser = function(user) {
      history.path('/administration/utilisateurs/edition/' + user.siteUserId);
    };

    return vm;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { userId: state.userId };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
