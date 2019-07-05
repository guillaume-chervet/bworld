import redux from '../../redux';

const init = function() {
  const menuItems = [];

  const state = redux.getState();
  const master = state.master;

  for (const menuKey in master.menu) {
    let menuTitle = '';
    switch (menuKey) {
      case 'bottomMenuItems':
        menuTitle = 'Bas';
        break;
      case 'privateMenuItems':
        menuTitle = 'Privée';
        break;
      case 'menuItems':
        menuTitle = 'Public';
        break;
      default:
        menuTitle = menuKey;
        break;
    }

    const mItems = master.menu[menuKey];
    for (let i = 0; i < mItems.length; i++) {
      const menuItem = mItems[i];
      menuItems.push({
        title: menuTitle + ' > ' + menuItem.title,
        id: menuItem.moduleId + menuItem.action,
      });

      if (menuItem.childs) {
        for (let j = 0; j < menuItem.childs.length; j++) {
          const child = menuItem.childs[j];
          menuItems.push({
            title: menuTitle + ' > ' + menuItem.title + ' > ' + child.title,
            id: child.moduleId + child.action,
          });
        }
      }
    }
  }
  return menuItems;
};

var getMenuItem = function(menutItems, data) {
  if (!data || !data.id) {
    return null;
  }

  for (let j = 0; j < menutItems.length; j++) {
    const menuItem = menutItems[j];
    if (menuItem.moduleId + menuItem.action === data.id) {
      return menuItem;
    } else if (menuItem.childs) {
      const mi = getMenuItem(menuItem.childs, data);
      if (mi !== null) {
        return mi;
      }
    }
  }

  return null;
};

const getPath = function(data) {
  const state = redux.getState();
  const master = state.master;
  for (const menuKey in master.menu) {
    const menuItems = master.menu[menuKey];
    const menuItem = getMenuItem(menuItems, data);
    if (menuItem) {
      if (data && data.anchor) {
        return `${menuItem.routePath}#${data.anchor}`;
      }
      return menuItem.routePath;
    }
  }
  return '#';
};

const getTitle = function(data, label) {
  if (label) {
    return label;
  }
  const _default = 'No title';

  if (data.type) {
    switch (data.type) {
      case 'facebook':
        return 'Facebook';
      case 'mail':
        return 'EMail';
      case 'phone':
        return 'Téléphone';
      default:
        return _default;
    }
  } else {
    const state = redux.getState();
    const master = state.master;
    for (var menuKey in master.menu) {
      const menuItem = getMenuItem(master.menu[menuKey], data);
      if (menuItem) {
        return menuItem.title;
      }
    }
  }

  return _default;
};

const addElement = function(parentElement, guid) {
  const newElement = {
    type: 'link',
    property: guid.guid(),
    label: 'Lien',
    data: {
      title: null,
      id: null,
    },
    $parent: parentElement,
  };
  return newElement;
};

export const service = {
  init: init,
  getPath: getPath,
  getTitle: getTitle,
  addElement,
};
