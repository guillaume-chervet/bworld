import redux from '../../redux';
import module from '../../adminSuper/modules/module-factory';
import _ from 'lodash';
import modulesFactory from '../../modules-factory';

const menu = {};

function canUp(menuItem, menuItems) {
  if (!menuItems || !menuItem) {
    return false;
  }
  return menuItems.indexOf(menuItem) > 0;
  
}

function canDown(menuItem, menuItems) {
  if (!menuItems || !menuItem) {
    return false;
  }
  return menuItems.indexOf(menuItem) < menuItems.length - 1;
}

function up(menuItem, menuItems) {
  if (menuItem && menuItems) {
    const index = menuItems.indexOf(menuItem);
    menuItems.splice(index, 1);
    menuItems.splice(index - 1, 0, menuItem);
  }
  return menuItems;
}

function down(menuItem, menuItems) {
  if (menuItem && menuItems) {
    const index = menuItems.indexOf(menuItem);
    menuItems.splice(index, 1);
    menuItems.splice(index + 1, 0, menuItem);
  }
  return menuItems;
}

function slideMenu(menuItem, menuItemsOrigin, menuItemsDestination) {
  const index = menuItemsOrigin.indexOf(menuItem);
  if (index > -1) {
    menuItemsOrigin.splice(index, 1);
    menuItemsDestination.push(menuItem);
  }
}

function canSetChild(menuItem, menuItemsOrigin) {
  if (menuItem.childs && menuItem.childs.length > 0) {
    return false;
  }
  const index = menuItemsOrigin.indexOf(menuItem);
  if (index < 1) {
    return false;
  }
  if (!modulesFactory.getModule(menuItem.module).canBeChild) {
    return false;
  }
  const newParent = menuItemsOrigin[index - 1];
  return modulesFactory.getModule(newParent.module).canHaveChild;
  
}

function setChild(menuItem, menuItemsOrigin) {
  const index = menuItemsOrigin.indexOf(menuItem);
  if (!canSetChild(menuItem, menuItemsOrigin)) {
    return;
  }
  const newParent = menuItemsOrigin[index - 1];
  if (!newParent.childs) {
    newParent.childs = [];
  }
  menuItemsOrigin.splice(index, 1);
  newParent.childs.push(menuItem);
}

function canSetParent(menuItem) {
  return modulesFactory.getModule(menuItem.module).canBeParent;
}

function setParent(menuItem, menuItemsOrigin, menuItemParent) {
  const index = menuItemParent.childs.indexOf(menuItem);
  menuItemParent.childs.splice(index, 1);
  const parentIndex = menuItemsOrigin.indexOf(menuItemParent);
  menuItemsOrigin.splice(parentIndex + 1, 0, menuItem);
}

function initModel(menuItems) {
  const menuItemsTemp = [];
  if (menuItems) {
    menuItems.forEach(menuItem => {
      const moduleId = menuItem.moduleId;
      const newItem = {
        moduleId: moduleId,
        parentId: null,
        childs: [],
      };
      menuItemsTemp.push(newItem);
      if (menuItem.childs) {
        menuItem.childs.forEach(function(element) {
          if (element.moduleId !== moduleId) {
            newItem.childs.push({
              moduleId: element.moduleId,
              parentId: moduleId,
            });
          }
        });
      }
    });
    return menuItemsTemp;
  }
  return null;
}

const init = function() {
  const state = redux.getState();
  Object.assign(menu, _.cloneDeep(state.master.menu));
};

function saveAsync() {
  const _menus = {};
  if (menu) {
    for (var name in menu) {
      _menus[name] = initModel(menu[name]);
    }
  }
  return module.saveMenuAsync(_menus);
}

const deleteElement = function(menuItem, menuItems) {
  if (menuItems) {
    while (menuItems.indexOf(menuItem) !== -1) {
      menuItems.splice(menuItems.indexOf(menuItem), 1);
    }
  }
  return menuItems;
};

export const menuAdmin = {
  init: init,
  saveAsync,
  menu: menu,
  up,
  down,
  canUp,
  canDown,
  slideMenu,
  setChild,
  canSetParent,
  setParent,
  canSetChild,
  deleteElement,
};
