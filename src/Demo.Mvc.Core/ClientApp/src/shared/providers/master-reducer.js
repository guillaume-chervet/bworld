import {
  MASTER_INIT,
  MASTER_UPDATE,
  MASTER_UPDATE_METAS,
  MASTER_UPDATE_MENU,
  MASTER_UPDATE_BREADCRUMB,
} from './master-types';
import history from '../../history';
import { concatUrl, master as masterUtils } from './master-provider';
import typeMenuItem from '../typeMenuItem';

export const isDisplayMenu = function(search) {
  const searchObject = search();
  let isDisplayMenu = true;
  let isDisplayDivMenu = true;
  if (!searchObject) {
    return { isDisplayMenu, isDisplayDivMenu };
  }
  if (searchObject.display_menu) {
    if (
      searchObject.display_menu === 'false' ||
      searchObject.display_menu === false
    ) {
      isDisplayDivMenu = false;
    }
  }

  if (searchObject.dm === 'false' || searchObject.dm === false) {
    isDisplayMenu = false;
  }
  return { isDisplayMenu, isDisplayDivMenu };
};

const params = window.params;

const updateMaster = search => (masterTemp, baseUrlSite) => {
  const data = init(masterTemp, baseUrlSite);
  const displayMenu = isDisplayMenu(search);
  return {
    masterData: data.masterData,
    masterServer: data.masterServer,
    menu: data.menu,
    menuData: {
      isCollapsed: true,
      ...displayMenu,
    },
    homePageInfo: _initHomePageInfo(masterTemp, 'menuItems'),
    homePagePrivateInfo: _initHomePageInfo(masterTemp, 'privateMenuItems'),
    routeCurrentModuleId: null,
    path: '',
  };
};

const initialState = updateMaster(history.search)(
  params.master,
  params.baseUrlSite
);

export const getInitialState = () => initialState;

function _initHomePageInfo(masterTemp, menuKey) {
  let moduleName;
  const homePageInfo = {};
  const menuItems = masterTemp[menuKey];
  if (menuItems && menuItems.length > 0) {
    for (var i = 0; i < menuItems.length; i++) {
      const mi = menuItems[i];
      if (mi.typeMenuItem == null || mi.typeMenuItem === typeMenuItem.page) {
        moduleName = mi.moduleName;
        homePageInfo.serviceName = moduleName;
        homePageInfo.menuKey = menuKey;
        homePageInfo.viewName = moduleName
          .replace(/^./, function(str) {
            return str.toLocaleLowerCase();
          })
          .replace(/([A-Z])/g, '-$1');
        return homePageInfo;
      }
    }
  } else {
    homePageInfo.serviceName = 'Default';
    homePageInfo.viewName = 'default';
  }
  return homePageInfo;
}

function init(masterTemp, baseUrlSite) {
  const masterServer = masterTemp;

  const menu = {
    menuItems: initMenu(masterTemp.menuItems, baseUrlSite),
    bottomMenuItems: initMenu(masterTemp.bottomMenuItems, baseUrlSite),
    privateMenuItems: initMenu(masterTemp.privateMenuItems, baseUrlSite),
  };

  const siteId = masterTemp.site.siteId;
  const masterData = {};
  masterData.titleSite = masterTemp.master.title;
  masterData.id = masterTemp.master.id;

  masterData.isJumbotron = masterTemp.master.isJumbotron;
  masterData.imageLogos = [];
  for (var i = 0; i < masterTemp.master.imageLogos.length; i++) {
    const item = masterTemp.master.imageLogos[i];
    const logo = {
      url:
        '/api/file/get/' + siteId + '/' + item.id + '/ImageThumb/' + item.name,
      description: item.description,
      title: item.title,
    };
    masterData.imageLogos.push(logo);
  }

  if (masterTemp.master.imageLogoId) {
    masterData.logoUrl =
      '/api/file/get/' +
      siteId +
      '/' +
      masterTemp.master.imageLogoId +
      '/ImageThumb/' +
      masterTemp.master.imageLogoFileName;
    masterData.isLogo = 'true';
  } else {
    masterData.isLogo = 'false';
  }
  if (masterTemp.master.imageIconeId) {
    masterData.iconeUrl =
      '/api/file/get/' +
      siteId +
      '/' +
      masterTemp.master.imageIconeId +
      '/ImageThumb/favicon.png';
  } else {
    masterData.iconeUrl = '/Content/images/favicon.png';
  }
  if (!masterData.titlePage) {
    masterData.titlePage = '';
  }
  masterData.facebookAuthenticationAppId =
    params.master.master.facebookAuthenticationAppId;
  masterData.styleTemplate = masterTemp.master.styleTemplate;
  return {
    masterData,
    masterServer,
    menu,
  };
}

function getUrlSite(baseUrlSite, path) {
  return concatUrl(baseUrlSite, path);
}

function initMenu(menuItemsSource, baseUrlSite) {
  const menuItemsDestination = [];
  if (menuItemsSource) {
    for (var i = 0; i < menuItemsSource.length; i++) {
      const menuItem = menuItemsSource[i];
      if (menuItem.routeDatas) {
        const routeDatas = menuItem.routeDatas;
        let routePath = '';
        if (
          menuItem.typeMenuItem &&
          menuItem.typeMenuItem === typeMenuItem.link
        ) {
          routePath = menuItem.routePath;
        } else {
          routePath = getUrlSite(baseUrlSite, '/' + menuItem.routePath);
        }
        const newItem = {
          routePath: routePath,
          adminRoutePath: getUrlSite(
            baseUrlSite,
            '/administration/' + menuItem.routePathWithoutHomePage
          ),
          title: menuItem.title,
          moduleId: routeDatas.moduleId,
          module: routeDatas.controller,
          action: routeDatas.action,
          state: menuItem.state,
          typeMenuItem: menuItem.typeMenuItem,
          icon: menuItem.icon,
        };
        menuItemsDestination.push(newItem);
        if (menuItem.childs) {
          newItem.childs = initMenu(menuItem.childs);
        }
      }
    }
  }
  return menuItemsDestination;
}

const master = (state = initialState, action) => {
  switch (action.type) {
    case MASTER_UPDATE: {
      const masterTemp = action.data;
      return updateMaster(history.search)(masterTemp, params.baseUrlSite);
    }
    case MASTER_UPDATE_METAS: {
      const data = action.data;
      return { ...state, masterData: { ...state.masterData, ...data } };
    }
    case MASTER_UPDATE_MENU: {
      const data = action.data;
      const path = data.path;
      const routeCurrentModuleId = data.routeCurrentModuleId;
      return {
        ...state,
        menuData: { ...state.menuData, ...data.menu },
        path,
        routeCurrentModuleId,
        breadcrumb: { items: null },
      };
    }
    case MASTER_UPDATE_BREADCRUMB: {
      return { ...state, breadcrumb: { items: action.data } };
    }
    default:
      return state;
  }
};

export default master;
