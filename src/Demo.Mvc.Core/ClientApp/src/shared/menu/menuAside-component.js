import app from '../../app.module';
import history from '../../history';
import redux from '../../redux';
import { userNotification } from '../../user/info/userNotification-factory';
import { user } from '../../user/info/user-factory';
import { menu } from './menu-factory';
import view from './menuAside.html';
import master from '../providers/master-reducer';
import { master as masterFactory } from '../providers/master-provider';

const name = 'menuAside';

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    const vm = this;

    vm.notification = userNotification.data;
    vm.isAdmin = menu.isAdmin;
    vm.isUser = menu.isUser;
      vm.isPrivate = menu.isPrivate;
      vm.getInternalPath = masterFactory.getInternalPath;

    vm.getUserName = function(initial) {
      const _user = vm.user;
      if (initial) {
        return (
          _user.firstName.slice(0, 1).toUpperCase() +
          _user.lastName.slice(0, 1).toUpperCase()
        );
      }
      return _user.firstName;
    };

    vm.getMainMenuItem = menu.getMainMenuItem;

    vm.isPrivateUser = function() {
      const _user = vm.user;
      if (!_user.isAuthenticate) {
        return false;
      }
      if (!_user.roles.isPrivateUser) {
        return false;
      }
      const menuItemsTemp = menu.getMenuItems(vm.menu.privateMenuItems, false);
      if (menuItemsTemp.length <= 0) {
        return false;
      }
      return true;
    };

    vm.logOff = function() {
      user.logOffAsync().then(function() {
        history.path('/utilisateur/connexion');
      });
    };
    vm.toggleMenu = function() {
      menu.updateMenu(!vm.isCollapsed);
    };

    return vm;
  }
  $onDestroy() {
    this.unsubscribe();
  }
    mapStateToThis(state) {  
    return {
      user: state.user.user,
       menu: menu.mapPublishedMenu(state.master.menu),
      isCollapsed: state.master.menuData.isCollapsed,
    };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: Controller,
  bindings: {
    currentPath: '<',
  },
});

export default name;
