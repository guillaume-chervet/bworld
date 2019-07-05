import app from '../../app.module';
import $window from '../../window';
import redux from '../../redux';
import { menu } from './menu-factory';
import view from './menuBottom.html';

const name = 'menuBottom';

const getMenuItems = function (master) {
  const newMenu = menu.mapPublishedMenu(master.menu);
  const menuItems = [];
    const menuItemsTemp = menu.getMenuItems(newMenu.bottomMenuItems, false);
  if (menuItemsTemp) {
    for (var i = 0; i < menuItemsTemp.length; i++) {
      menuItems.push(menuItemsTemp[i]);
    }
  }
  return menuItems;
};

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    const ctrl = this;
    ctrl.version = $window.params.version;
    ctrl.date = new Date();
    ctrl.isActive = menu.isActive;
    return ctrl;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    const master = state.master;
    return {
      menuItems: getMenuItems(master),
      titleSite: master.masterData.titleSite,
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
