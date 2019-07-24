import app from '../../app.module';
import route from '../../route';
import redux from '../../redux';
import typeMenuItem from '../typeMenuItem';
import {
  masterUpdate,
  masterUpdateMetas,
  masterUpdateMenu,
    masterUpdateBreadcrumb,
} from './master-actions';
import history from "../../history";

const name = 'Master';

const params = window.params;

const getFirstMenuItem = (menutItems) => {
  if (menutItems && menutItems.length > 0) {
    for (var i = 0; i < menutItems.length; i++) {
      const mi = menutItems[i];
      if (mi.typeMenuItem == null || mi.typeMenuItem === typeMenuItem.page) {
        return mi;
      }
    }
  }
};

const getModuleIdClean = (menuKey, routeCurrentModuleId, menu) => {
  if (!menuKey) {
    menuKey = 'menuItems';
  }
  if (routeCurrentModuleId) {
    return routeCurrentModuleId;
  }
  const mi = getFirstMenuItem(menu[menuKey]);
  if (mi) {
    return mi.moduleId;
  }
  const mib = getFirstMenuItem(menu.bottomMenuItem);
  if (mib) {
    return mib.moduleId;
  }
};

const getRouteCurrentModuleId = () => {
  const current = route.current();
  if (current) {
    const routeCurrentModuleId = current.params.moduleId;
    if (routeCurrentModuleId) {
      return routeCurrentModuleId;
    }
  }
  return null;
};

const getModuleId = ($route, menuKey) => {
  const state = redux.getState();
  const menu = state.master.menu;
  return getModuleIdClean(menuKey, getRouteCurrentModuleId(), menu);
};

const getCurrentMenuItemClean = (routeCurrentModuleId, menu) => {
  var moduleId = getModuleIdClean('', routeCurrentModuleId, menu);
  for (var name in menu) {
    const items = menu[name];
    if (items instanceof Array) {
      const item = getMenuItems(items, moduleId);
      if (item) {
        return item;
      }
    }
  }
  return null;
};
const getCurrentMenuItem = () => {
  const state = redux.getState();
  const menu = state.master.menu;
  getCurrentMenuItemClean(getRouteCurrentModuleId(), menu);
};

function getMenuItems(items, moduleId) {
  for (let i = 0; i < items.length; i++) {
    const menuItem = items[i];
    if (menuItem.moduleId === moduleId) {
      return menuItem;
    }
    if (menuItem.childs) {
      const item = getMenuItems(menuItem.childs, moduleId);
      if (item) {
        return item;
      }
    }
  }
  return null;
}

function getServerMenuItem(moduleId) {
  const items = params.master.menuItems;
  return getMenuItems(items, moduleId);
}

function getMasterServer() {
  const state = redux.getState();
  return state.master.masterServer;
}

function endsWith(str, suffix) {
  return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

function startsWith(str, suffix) {
  return str.indexOf(suffix) === 0;
}

function getUrl(path) {
  if (path === undefined) {
    return '';
  }
  if (path.indexOf('.html') != -1) {
    if (!startsWith(path, '/')) {
      path = '/' + path;
    }
    return concatUrl(null, path);
  }
  return concatUrl(params.baseUrlJs, path);
}

const getInternalPath = (path) => {

    if (path === undefined) {
        return '';
    }
    if (path.indexOf(params.baseUrlSite) != -1) {
        return path;
    }
    return concatUrl(params.baseUrlSite, path);
};

function getFullUrl(path) {
  return concatUrl(params.baseUrl, path);
}

export function concatUrl(base, path) {
  if (!base) {
    return path;
  }
  let urlBase = base;
  if (endsWith(base, '/')) {
    urlBase = base.slice(0, base.length - 1);
  }
  if (startsWith(path, '/')) {
    path = path.slice(1, path.length);
  }
  return urlBase + '/' + path;
}

export const master = {
  concatUrl,
  getModuleId,
  getModuleIdClean,
  getCurrentMenuItem,
  getCurrentMenuItemClean,
    getServerMenuItem,
  
  site: params.master.site,
  getMasterServer,
  updateMaster: masterTemp => {
    const dispatch = redux.getDispatch();
    dispatch(masterUpdate(masterTemp));
  },
  updateMasterMetas: data => {
    const dispatch = redux.getDispatch();
    dispatch(masterUpdateMetas(data));
  }, 
    updateMasterBreadcrumb: items => {
        const dispatch = redux.getDispatch();
        dispatch(masterUpdateBreadcrumb(items));
    },
  getRouteCurrentModuleId,
  updateMasterMenu: data => {
    const dispatch = redux.getDispatch();
    
    const dataToDisptach = {
      routeCurrentModuleId: getRouteCurrentModuleId(),
      path: history.path(),
      menu:data,
    };
    
    dispatch(masterUpdateMenu(dataToDisptach));
  },
  getUrl: getUrl,
    getFullUrl: getFullUrl,
    getInternalPath,
};

app.provider(name, [
  function() {
    return {
      getUrl,
      $get: function() {
        return master;
      },
    };
  },
]);

export default name;
