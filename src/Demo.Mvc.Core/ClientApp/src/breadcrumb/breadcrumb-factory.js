import history from '../history';
import redux from '../redux';
import rootScope from '../rootScope';
import { master } from '../shared/providers/master-provider';
import { menu } from '../shared/menu/menu-factory';
import route from '../route';
import _ from 'lodash';

const items = [];
const page = {
  title: null,
};

rootScope.$on('$locationChangeSuccess', function(/*event, newUrl, oldUrl*/) {
  items.length = 0;
});

const getMainItemPure = (master) =>  {
  const menuItem = menu.getMainMenuItemPure(master);
  const mainItem = {
    url: '/',
    title: menuItem.title,
    active: false,
    module: 'Home',
    icon: menuItem.icon,
  };
  return mainItem;
};
const getMainItem = () =>  {
  const state = redux.getState();
  const master = state.master;
  return getMainItemPure(master);
};

const getItemsClean = (path, _master, routeCurrentModuleId) => {
  const items = [];
  const _menu = _master.menu;
  
  if(_master.breadcrumb.items){
    return _master.breadcrumb.items;
  }
  
    if (path.indexOf('super-administration') !== -1) {
      items.push({
        url: '/super-administration',
        title: 'Super administration',
        active: false,
        module: 'SuperAdministration',
      });
    } else if (menu.isAdmin()) {
      items.push({
        url: '/administration',
        title: 'Administration',
        active: false,
        module: 'Administration',
      });
    } else if (path.indexOf('utilisateur') !== -1) {
      items.push({
        url: '/utilisateur',
        title: 'Utilisateur',
        active: false,
        module: 'User',
      });
    } else {
      if (menu.isPrivatePure(path)) {
        items.push(getPrivateItemClean(_master));
      } else {
        items.push(getMainItemPure(_master));
      }
    }
    const currentUrl = path;
    const itemFound = _.find(items, (i)  => i.url === currentUrl);

    if (!itemFound) {
      const currentMenuItem = master.getCurrentMenuItemClean(routeCurrentModuleId,_menu);
      if(currentMenuItem) {
        const titleBreabcrumb = page.title;
        const title = titleBreabcrumb
            ? titleBreabcrumb
            : _master.master
                ? _master.master.titlePage
                : 'Not found';
        items.push({
          url: currentUrl,
          title,
          active: true,
          module: currentMenuItem.module,
          icon: currentMenuItem.icon,
        });
      }
    }
  return items;
};
const getItems = () => {
  const state = redux.getState();
  const master = state.master;
  if (items.length === 0) {
    const newItems = getItemsClean(history.path(), master, route);
    newItems.forEach(i => items.push(i));
  }
  return items;
};

const setItems = (newItems) => {
  items.length = 0;
  if (newItems) {
    for (var i = 0; i < newItems.length; i++) {
      if (i > 0) {
        const j = i - 1;
        const isOk = j <= items.length - 1;
        if (!isOk || items[i - 1].module !== newItems[i].module) {
          items.push(newItems[i]);
        }
      } else {
        items.push(newItems[i]);
      }
    }
  }
  master.updateMasterBreadcrumb(items);
};

const isVisible= () => {
  const state = redux.getState();
  return isVisibleClean(state.master, history.path());
};

const isVisibleClean = (master, path) => {
  const isMenuVisible = master.menuData.isDisplayMenu;
  if (!isMenuVisible) {
    return false;
  }
  const privateMenuItem = menu.getMainMenuItemPure(master, 'privateMenuItems');
  const privatePath = privateMenuItem ? privateMenuItem.url : '';
  return (
      path !== '/' &&
      path !== '' &&
      path !== privatePath
  );
};

function getAdminItem() {
  const item = {
    url: '/administration',
    title: 'Administration',
    active: false,
    module: 'Administration',
  };
  return item;
}

const getPrivateItemClean = (master) => {
  const menuItem = menu.getMainMenuItemPure('privateMenuItems', master);
  const item = {
    url: menuItem.routePath,
    title: menuItem.title,
    active: false,
    module: 'Private',
    icon: menuItem.icon,
  };
  return item;
};
const getPrivateItem = () => {
  const state = redux.getState();
  const master = state.master;
  return getPrivateItemClean(master);
};

const getLdJson = () => {
  getLdJsonClean(items);
};

const getLdJsonClean = (items) => {
  const itemListElement = [];
  const breadcrumbJson = {
    '@context': 'http://schema.org',
    '@type': 'BreadcrumbList',
    itemListElement: itemListElement,
  };
  for (let i = 0; i < items.length; i++) {
    const item = items[i];
    const itemJson = {
      '@type': 'ListItem',
      position: i + 1,
      item: {
        '@id': master.getFullUrl(item.url), // 'https://example.com/books',
        name: item.title,
        //'image': 'http://example.com/images/icon-book.png' //TODO SEO icon
      },
    };
    itemListElement.push(itemJson);
  }
  return breadcrumbJson;
};

const navBack = (search) => {
  const length = items.length;
  if (length > 1) {
    const item = items[length - 2];
    if (search) {
      history.search(search, item.url);
    }
  }
};

export const breadcrumb = {
  getItemsClean,
  getItems,
  setItems,
  isAdmin: menu.isAdmin,
  isVisibleClean,
  isVisible,
  page,
  getMainItem,
  getPrivateItem,
  getAdminItem,
  getLdJsonClean,
  getLdJson,
  navBack,
};
