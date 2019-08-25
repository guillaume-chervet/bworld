import redux from '../../redux';
import history from '../../history';
import rootScope from '../../rootScope';
import itemStates from '../itemStates';
import typeMenuItem from '../typeMenuItem';
import { master } from '../providers/master-provider';
import { isDisplayMenu } from '../providers/master-reducer';
import _ from 'lodash';

const updateMenu = isCollapsed => {
  if (isCollapsed === undefined) {
    const state = redux.getState();
    isCollapsed = !state.master.menuData.isCollapsed;
  }
  const _menu = {
    isCollapsed,
    ...isDisplayMenu(history.search),
  };
  master.updateMasterMenu(_menu);
};

rootScope.$on('$locationChangeSuccess', function() {
  updateMenu(true);
});

const getMainMenuItemPure = (master, propertyName) => {
  if (!propertyName) {
    propertyName = 'menuItems';
  }
  const menuItems = getMenuItems(master.menu[propertyName], false);
  if (menuItems && menuItems.length > 0) {
    return _.find(menuItems, function(mi) {
      return mi.typeMenuItem == null || mi.typeMenuItem === typeMenuItem.page;
    });
  }
  return null;
};
const getMainMenuItem = (propertyName) => {
  const state = redux.getState();
  const master = state.master;
  return getMainMenuItemPure(master, propertyName);
};

const isAdminPure = (path) => path.indexOf('/administration') === 0;
const isAdmin = () => isAdminPure(history.path());

const isPrivatePure = (path) => (
      path.indexOf('/privee') === 0 ||
      path.indexOf('/administration/privee') === 0
  );
const isPrivate = () => isPrivatePure(history.path());

const isUserPure = (path) => path.indexOf('/utilisateur') === 0;
const isUser = () => isUserPure(history.path());

function isActive(routePath, currentPath) {
  if (!currentPath) {
    currentPath = history.path();
  }
  if (routePath === '/') {
    if (currentPath === '/') {
      return true;
    }
  } else if (currentPath.slice(0, routePath.length) === routePath) {
    return true;
  }
  return false;
}

function getMenuItems(originMenuItems, limitLength) {
  if (!limitLength) {
    limitLength = 3;
  }
  if (!originMenuItems) {
    throw 'originMenuItems not set';
  }
  return getSecondMenuItems(originMenuItems, limitLength, 0, null);
}

const getStateMenuItems = function(menuItems, itemState) {
  if (!menuItems) {
    return menuItems;
  }
  if (itemState === undefined) {
    return menuItems;
  }

  const result = [];
  if (menuItems.length > 0) {
    menuItems.forEach(function(mi) {
      let miDraft = mi.state;
      if (miDraft === undefined || miDraft === 0) {
        miDraft = itemStates.published;
      }
      if (miDraft === itemState) {
        result.push(mi);
      }
    });
  }
  return result;
};

function getSecondMenuItems(
  originMenuItems,
  limitLength,
  startLength,
  module,
  menuItems
) {
  if (!menuItems) {
    menuItems = [];
  } else {
    menuItems.length = 0;
  }

  if (startLength === undefined) {
    startLength = 1;
  }
  if (originMenuItems) {
    const menuItemsTemp = getStateMenuItems(
      originMenuItems,
      itemStates.published
    );
    const length = menuItemsTemp.length;
    if (length > startLength) {
      if (!limitLength) {
        limitLength = 3;
      }

      if (length < limitLength) {
        limitLength = length;
      }

      for (let i = startLength; i < limitLength; i++) {
        const mi = menuItemsTemp[i];
        if (module) {
          if (mi.module === module) {
            menuItems.push(mi);
          }
        } else {
          menuItems.push(mi);
        }
      }
    }
  }
  return menuItems;
}

const mapPublishedMenu = (oldMenu) => {
    //const oldMenu = state.master.menu;
    const newMenu = {};
    for (var property in oldMenu) {
            newMenu[property] = getStateMenuItems(oldMenu[property], itemStates.published);
    }
    return newMenu;
};

export const menu = {
  getSecondMenuItems,
  isActive,
  getMenuItems,
  getMainMenuItemPure,
    getMainMenuItem,
    getStateMenuItems,
  isAdmin,
  isAdminPure,
  isPrivate,
  isPrivatePure,
  isUserPure,
  isUser,
    updateMenu,
    mapPublishedMenu,
};
