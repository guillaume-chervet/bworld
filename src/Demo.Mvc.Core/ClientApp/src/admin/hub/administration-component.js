import app from '../../app.module';
import history from '../../history';
import modulesFactory from '../../modules-factory';
import { page } from '../../shared/services/page-factory';
import { master } from '../../shared/providers/master-provider';
import itemStates from '../../shared/itemStates';
import { userNotification } from '../../user/info/userNotification-factory';
import { administration } from './administration-factory';
import { dialogAdd } from './addModule/dialogAdd-factory';

import view from './administration.html';

import './administration.css';

const name = 'administration';

class Controller {
  $onInit() {
    const vm = this;
    page.setTitle('Accueil', page.types.admin);

    vm.getInternalPath = master.getInternalPath;

    vm.administration = administration.administration;
    vm.notification = {
      numberSiteUnreadMessage: userNotification.data.numberSiteUnreadMessage,
    };

    function initMenu(menuItemsSource, menuItemsDestination) {
      if (menuItemsSource && menuItemsDestination) {
        for (let i = 0; i < menuItemsSource.length; i++) {
          const menuItem = menuItemsSource[i];
          const service = modulesFactory.getModule(
            menuItem.routeDatas.controller
          );
          if (service && service.service && service.service.initMenuAdmin) {
            service.service.initMenuAdmin(menuItemsDestination, menuItem);
          }
        }
      }
    }

    vm.getModule = modulesFactory.getModule;

    function getMenuAdmin() {
      const menu = {
        menuItems: [],
        bottomMenuItems: [],
        privateMenuItems: [],
      };
      const masterTemp = master.getMasterServer();

      initMenu(masterTemp.menuItems, menu.menuItems);
      initMenu(masterTemp.bottomMenuItems, menu.bottomMenuItems);
      initMenu(masterTemp.privateMenuItems, menu.privateMenuItems);

      return menu;
    }

    vm.menu = getMenuAdmin();

    vm.addModule = function(propertyName, mode) {
      dialogAdd.openAsync(propertyName, mode);
    };

    vm.goMenu = function() {
      history.path('/administration/menu');
    };

    vm.getClass = function(menuItem) {
      if (!menuItem) {
        return '';
      }
      if (menuItem.state === itemStates.draft) {
        return 'mw-draft';
      }
      return '';
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
