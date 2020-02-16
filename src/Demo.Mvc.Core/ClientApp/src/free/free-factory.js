import _ from 'lodash';

import modulesFactory from '../modules-factory';
import { master } from '../shared/providers/master-provider';
import itemStates from '../shared/itemStates';
import { service as elementService } from '../elements/element-factory';
import icons from '../shared/icons';
import { module } from '../adminSuper/modules/module-factory';
import $http from '../http';

const metaElementDefault = {
  type: 'metaDescription',
  property: 'MetaDescription',
  label: 'Description qui apparaît dans les moteurs de recherche',
  data: '',
};

const createData = function() {
  const newData = {
    moduleId: null,
    elements: [],
    metaElements: [],
    userInfo: {},
    lastUpdateUserInfo: {},
    createDate: null,
    updateDate: null,
    data: {
      state: itemStates.published,
      tags: [],
      isDisplayAuthor: false,
      isDisplaySocial: false,
      icon: null,
    },
  };
  return newData;
};

const data = createData();
const elements = data.elements;
const metaElements = data.metaElements;
metaElements.push(metaElementDefault);

const init = function(newElements) {
  elements.length = 0;
  newElements.forEach(e => elements.push(e));
};

const getElement = function(elements, propertyName) {
  for (var i = 0; i < elements.length; i++) {
    if (elements[i].property === propertyName) {
      return elements[i];
    }
  }
  return null;
};

const initData = function(result, data) {
  const moduleId = master.getModuleId();
  if (result.userInfo) {
    Object.assign(data.userInfo, _.cloneDeep(result.userInfo));
  }
  if (result.lastUpdateUserInfo) {
    Object.assign(
      data.lastUpdateUserInfo,
      _.cloneDeep(result.lastUpdateUserInfo)
    );
  }
  data.createDate = result.createDate;
  data.updateDate = result.updateDate;
  data.data.state = result.state;
  data.data.isDisplayAuthor = result.isDisplayAuthor;
  data.data.isDisplaySocial = result.isDisplaySocial;
  data.data.icon = result.icon;
  data.data.tags.length = 0;
  if (result.tags) {
    result.tags.forEach(tag => {
      data.data.tags.push(tag);
    });
  }
  data.elements.length = 0;
  if (!result.elements) {
    const elem = [];
    const h1 = {
      type: 'h1',
      property: 'h1',
      label: 'Titre pincipal',
      data: '',
    };
    const p = {
      type: 'p',
      property: 'p',
      label: 'Paragraphe',
      data: '',
    };
    elem.push(h1);
    elem.push(p);
    elem.push(metaElementDefault);
    result.elements = elem;
  }
  elementService.initDataElement(
    moduleId,
    result.elements,
    data.elements,
    data.metaElements
  );
};

const initAsync = function(menuKey) {
  const siteId = master.site.siteId;
  const moduleId = master.getModuleId(null, menuKey);
  return $http
    .get(master.getUrl('api/free/get/' + siteId + '/' + moduleId))
    .then(function(response) {
      elements.length = 0;
      if (response) {
        const result = response.data.data;
        initData(result, data);
      }
    });
};

function mapParent(elementParent) {
  if (!elementParent) {
    return elementParent;
  }
  if (!elementParent.$parent) {
    elementParent.$level = 0;
  }
  if (!elementParent.childs || elementParent.childs === null) {
    return elementParent;
  }
  const childs = elementParent.childs;
  childs.forEach(element => {
    element.$parent = elementParent;
    element.$level = elementParent.$level + 1;
    mapParent(element);
  });
  return elementParent;
}

const saveAsync=(moduleId, menuPropertyName, parentId) => {
  const elementsTemp = elementService.mapElement(elements, metaElements);
  const dataToSend = {
    parentId: parentId,
    site: master.site,
    moduleId: moduleId,
    propertyName: menuPropertyName,
    elements: elementsTemp,
    state: data.data.state,
    isDisplayAuthor: data.data.isDisplayAuthor,
    isDisplaySocial: data.data.isDisplaySocial,
    icon: data.data.icon,
  };
  const promise = $http
    .post(master.getUrl('api/free/save'), dataToSend)
    .then(function(response) {
      if (response.data.isSuccess) {
        master.updateMaster(response.data.data.master);
      }
      return response.data;
    });
  module.saveSuccess(promise, moduleId);
  return promise;
};

const initMenuAdmin = function(menuItems, menuItem) {
  const newMenuItem = {
    routePath: master.getInternalPath(
      '/administration/' + menuItem.routePathWithoutHomePage
    ),
    title: menuItem.title,
    module: 'Free',
    moduleId: menuItem.routeDatas.moduleId,
    icon: menuItem.icon,
    state: menuItem.state,
  };
  menuItems.push(newMenuItem);
  if (menuItem.childs) {
    newMenuItem.childs = [];
    menuItem.childs.forEach(function(element) {
      const service = modulesFactory.getModule(element.routeDatas.controller);
      if (service && service.service && service.service.initMenuAdmin) {
        service.service.initMenuAdmin(newMenuItem.childs, element);
      }
    });
  }
};

const isUploading = (elements) => {
  if (!elements) {
    return false;
  }
  for (let i = 0; i < elements.length; i++) {
    const element = elements[i];
    if (
      element.type === 'file' ||
      element.type === 'image' ||
      element.type === 'carousel'
    ) {
      const clientFiles = element.data;
      for (let j = 0; j < clientFiles.length; j++) {
        const clientFile = clientFiles[j];
        if (!clientFile.propertyName) {
          return true;
        }
      }
    } else {
      if (element.childs) {
        const result = isUploading(element.childs);
        if (result === true) {
          return true;
        }
      }
    }
  }

  return false;
};

const getTitle = (elements) => {
  if (elements) {
    const elem = getFirstElement(elements, ['h1'], null);
    if (elem) {
      return elem.data;
    }
  }
  return 'Sans titre';
};

const getFirstParagraph = (elements) => {
  return getFirstElement(elements, ['p'], null);
};

const getFirstImage = (elements) => {
  return getFirstElement(elements, ['image', 'file', 'carousel'], null);
};

const getFirstElement = (elements, types, defaultValue) => {
  if (elements) {
    for (var i = 0; i < elements.length; i++) {
      const element = elements[i];
      for (var j = 0; j < types.length; j++) {
        if (element.type === types[j]) {
          return element;
        }
      }
      const childs = element.childs;
      if (childs) {
        const result = getFirstElement(childs, types, defaultValue);
        if (result !== defaultValue) {
          return result;
        }
      }
    }
  }
  return defaultValue;
};

const addAsync = (propertyName, icon) => {
  let elements = [
    {
      type: 'h1',
      property: 'Title1',
      label: 'Titre',
      data: 'Titre de la page',
    },
    {
      type: 'p',
      property: 'p1',
      label: 'Text',
      data: 'Paragraphe de la page',
    },
    {
      type: 'file',
      property: 'file1',
      label: 'Text',
      data: [],
    },
  ];
  if (icon === icons.Contact) {
    elements = [
      { type: 'h1', property: 'Contact', label: 'Titre principal', data: '' },
      { type: 'p', property: 'Paragraphe', label: 'Description', data: '' },
      {
        type: 'message',
        property: 'Titres des messages',
        label: 'Description',
        data: [],
      },
    ];
  }

  init(elements);
  data.data.icon = icon;
  return saveAsync(null, propertyName, null);
};

export const free = {
  addAsync: addAsync,
  initAsync: initAsync,
  saveAsync: saveAsync,
  elements: elements,
  data: data,
  init: init,
  mapElement: elementService.mapElement,
  initMenuAdmin: initMenuAdmin,
  isUploading: isUploading,
  getElement: getElement,
  mapParent: mapParent,
  initData: initData,
  createData: createData,
  getTitle: getTitle,
  getFirstParagraph: getFirstParagraph,
  getFirstImage: getFirstImage,
  getFirstElement: getFirstElement,
};
