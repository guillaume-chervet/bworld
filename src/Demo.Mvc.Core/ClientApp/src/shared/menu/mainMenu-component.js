import app from '../../app.module';
import history from '../../history';
import { user } from '../../user/info/user-factory';
import redux from '../../redux';
import { userNotification } from '../../user/info/userNotification-factory';
import { menu } from './menu-factory';
import { master  } from '../providers/master-provider';
import view from './mainMenu.html';

const name = 'mainMenu';

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
    vm.updateMenu = () => menu.updateMenu(!vm.isCollapsed);
    vm.isAdmin = menu.isAdmin;
      vm.isUser = menu.isUser;
      vm.getInternalPath = master.getInternalPath;

    vm.isPrivate = menu.isPrivate;
    vm.getUserName = function(initial) {
      var _user = vm.user;
      if (initial) {
        return (
          _user.firstName.slice(0, 1).toUpperCase() +
          _user.lastName.slice(0, 1).toUpperCase()
        );
      }
      return _user.firstName;
    };
    vm.getMainMenuItem = menu.getMainMenuItem;
    vm.logOff = function() {
      user.logOffAsync().then(function() {
        history.path('/utilisateur/connexion');
      });
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
      isDisplayMenu: state.master.menuData.isDisplayMenu,
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
