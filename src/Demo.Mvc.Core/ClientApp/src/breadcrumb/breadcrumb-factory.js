import history from '../history';
import redux from '../redux';
import rootScope from '../rootScope';
import { master } from '../shared/providers/master-provider';
import { menu } from '../shared/menu/menu-factory';
import _ from 'lodash';

const items = [];
const page = {
  title: null,
};

rootScope.$on('$locationChangeSuccess', function(/*event, newUrl, oldUrl*/) {
  items.length = 0;
});

function getMainItem() {
  const menuItem = menu.getMainMenuItem();
  const mainItem = {
    url: '/',
    title: menuItem.title,
    active: false,
    module: 'Home',
    icon: menuItem.icon,
  };
  return mainItem;
}

function getItems() {
  if (items.length === 0) {
    if (history.path().indexOf('super-administration') !== -1) {
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
    } else if (history.path().indexOf('utilisateur') !== -1) {
      items.push({
        url: '/utilisateur',
        title: 'Utilisateur',
        active: false,
        module: 'User',
      });
    } else {
      if (menu.isPrivate()) {
        items.push(getPrivateItem());
      } else {
        items.push(getMainItem());
      }
    }
    const currentUrl = history.path();
    const itemFound = _.find(items, function(i) {
      return i.url === currentUrl;
    });

    if (!itemFound) {
      const currentMenuItem = master.getCurrentMenuItem();
      const titleBreabcrumb = page.title;
      const title = titleBreabcrumb
        ? titleBreabcrumb
        : master.master
        ? master.master.titlePage
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
}

function setItems(newItems) {
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
}

function isVisible() {
  const state = redux.getState();
  const isMenuVisible = state.master.menuData.isDisplayMenu;
  if (!isMenuVisible) {
    return false;
  }
  const privateMenuItem = menu.getMainMenuItem('privateMenuItems');
  const privatePath = privateMenuItem ? privateMenuItem.url : '';
  return (
    history.path() !== '/' &&
    history.path() !== '' &&
    history.path() !== privatePath
  );
}

function getAdminItem() {
  const item = {
    url: '/administration',
    title: 'Administration',
    active: false,
    module: 'Administration',
  };
  return item;
}

function getPrivateItem() {
  const menuItem = menu.getMainMenuItem('privateMenuItems');
  const item = {
    url: menuItem.routePath,
    title: menuItem.title,
    active: false,
    module: 'Private',
    icon: menuItem.icon,
  };
  return item;
}

function getLdJson() {
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
}

function navBack(search) {
  if (search) {
    for (var name in search) {
      history.search(name, search[name]);
    }
  }
  const length = items.length;
  if (length > 1) {
    const item = items[length - 2];
    history.path(item.url);
  }
}

export const breadcrumb = {
  getItems,
  setItems,
  isAdmin: menu.isAdmin,
  isVisible,
  page,
  getMainItem,
  getPrivateItem,
  getAdminItem,
  getLdJson,
  navBack,
};
