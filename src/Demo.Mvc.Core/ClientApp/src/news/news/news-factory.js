import history from '../../history';
import { module as _module } from '../../adminSuper/modules/module-factory';
import { urlHelper } from '../../shared/services/urlHelper-factory';
import { master } from '../../shared/providers/master-provider';
import itemStates from '../../shared/itemStates';
import { menu } from '../../shared/menu/menu-factory';
import { breadcrumb } from '../../breadcrumb/breadcrumb-factory';
import { free } from '../../free/free-factory';
import { userData } from '../../user/info/userData-factory';
import { getScore } from '../../elements/form/elementForm-factory';
import $http from '../../http';
import $route from '../../route';
import { dialogTags } from '../../admin/tags/dialogTags-factory';

import _ from 'lodash';

const data = free.createData();
data.displayMode = 'article';
data.numberItemPerPage = 10;

const elements = data.elements;
elements.push({
  type: 'h1',
  property: 'Title',
  label: 'Titre principal',
  data: '',
});
elements.push({
  type: 'p',
  property: 'Paragraphe',
  label: 'Description',
  data: '',
});

const metaElements = data.metaElements;
metaElements.push({
  type: 'metaDescription',
  property: 'MetaDescription',
  label: 'Description qui apparaît dans les moteurs de recherche',
  data: '',
});

const getModuleId = function(menuKey) {
  return master.getModuleId(null, menuKey);
};

const init = function(newElements) {
  elements.length = 0;
  newElements.forEach(function(element) {
    elements.push(element);
  });
};

const mapItem = function(item) {
  const itemData = free.createData();
  itemData.metaElements.push({
    type: 'metaDescription',
    property: 'MetaDescription',
    label: 'Description qui apparaît dans les moteurs de recherche',
    data: '',
  });
  free.initData(item, itemData);
  const search = _.cloneDeep(history.search());

  let customImage = null;
  const formElement = free.getFirstElement(itemData.elements, ['form'], null);
  let userItemState = null;
  if (formElement) {
    const uData = userData.getData(item.moduleId, formElement.property);
    if (uData && uData.json) {
      userItemState = uData.json.state;
    }
    let imageUrl = null;
    if (
      formElement.data &&
      formElement.data.type &&
      formElement.data.type === 'training'
    ) {
      imageUrl = master.getUrl('/App/news/news/form-training.png');
    } else {
      imageUrl = master.getUrl('/App/news/news/form-test.png');
      search.dm = 'false';
    }
    customImage = {
      data: [
        { displayType: 'image', thumbnailUrl: imageUrl, behavior: 'noZoom' },
      ],
      type: 'file',
    };
  }

  const queryString = getQueryString(search);

  const title = free.getTitle(item.elements);
  let viewUrl = `/articles/item/${item.moduleId}/${urlHelper.normaliseUrl(
    title
  )}${queryString}`;
  if (menu.isPrivate()) {
    viewUrl = `/privee${viewUrl}`;
  }
  const editUrl = `/administration${viewUrl}`;

  const newItem = {};

  const newsElements = [];
  const paragraph = free.getFirstParagraph(itemData.elements);
  if (paragraph) {
    newsElements.push(paragraph);
  }

  const firstImage = free.getFirstImage(itemData.elements);
  let image = customImage;
  if (firstImage) {
    image = firstImage;
  }
  if (image) {
    newsElements.push(image);
  }
  const hasNext = itemData.elements.length > newsElements.length + 1;
  newItem.element = free.mapParent({
    type: 'div',
    childs: newsElements,
  });
  newItem.data = {
    editUrl: master.getInternalPath(editUrl),
    viewUrl: master.getInternalPath(viewUrl),
    state: item.state,
    tags: dialogTags
      .initTags(dialogTags.model.items.tags, [], itemData.data.tags)
      .filter(tag => tag.ticked),
    userScore: getScore(userItemState),
    isDisplayAuthor: item.isDisplayAuthor,
    isDisplaySocial: item.isDisplaySocial,
    userInfo: itemData.userInfo,
    lastUpdateUserInfo: itemData.lastUpdateUserInfo,
    title: title,
    createDate: itemData.createDate,
    updateDate: itemData.updateDate,
    hasNext: hasNext,
    numberComments: item.numberComments,
  };

  return newItem;
};

const addNewsItem = function(item) {
  const mappedItem = mapItem(item);
  data.items.splice(0, 0, mappedItem);
  return mappedItem;
};

const getQueryString = function(search) {
  let isFirst = true;
  const getSeparator = () => {
    if (isFirst) {
      isFirst = false;
      return '?';
    }
    return '&';
  };
  let getQueryString = '';
  for (const name in search) {
    const value = search[name];
    if (value) {
      if (Array.isArray(value)) {
        value.forEach(v => (getQueryString += `${getSeparator()}${name}=${v}`));
      } else {
        getQueryString += `${getSeparator()}${name}=${value}`;
      }
    }
  }

  return getQueryString;
};

const initAsync = function(menuKey, states) {
  if (states === undefined) {
    states = [itemStates.published];
  }

  const search = history.search();
  const searchCloned = _.cloneDeep(search);
  searchCloned.states = states;
  const queryString = getQueryString(searchCloned);

  const siteId = master.site.siteId;
  const moduleId = getModuleId(menuKey);
  const title = $route.current().params.title;

  let url = `api/articles/get/${siteId}/${moduleId}`;
  url += queryString;
  return $http.get(master.getUrl(url)).then(function(response) {
    if (response) {
      const result = response.data.data;
      free.initData(result, data);
      data.items = [];

      if (result.getNewsItem) {
        result.getNewsItem.forEach(function(item) {
          data.items.push(mapItem(item));
        });

        const isAdmin = breadcrumb.isAdmin();
        data.hasNext = result.hasNext;
        const searchClonedNext = _.cloneDeep(search);
        searchClonedNext.index = result.idNext;
        const queryStringNext = getQueryString(searchClonedNext);
        let urlNext = `/${queryStringNext}`;
        let urlPrevious = '/';
        const currentPath = history.path();
        if (currentPath !== '/') {
          urlNext = `/articles/${moduleId}/${title}${queryStringNext}`;
          urlPrevious = `/articles/${moduleId}/${title}`;
          if (menu.isPrivate()) {
            urlNext = `/privee${urlNext}`;
            urlPrevious = `/privee${urlPrevious}`;
          }
          if (isAdmin) {
            urlNext = `/administration${urlNext}`;
            urlPrevious = `/administration${urlPrevious}`;
          }
        }
        if (result.idPrevious) {
          const searchClonedPrevious = _.cloneDeep(search);
          searchClonedPrevious.index = result.idPrevious;
          urlPrevious += `?${getQueryString(searchClonedPrevious)}`;
        }

        data.urlNext = master.getInternalPath(urlNext);
        data.hasPrevious = result.hasPrevious;
        data.urlPrevious = master.getInternalPath(urlPrevious);
        data.displayMode = result.displayMode;
        data.numberItemPerPage = result.numberItemPerPage;
      }
    }
  });
};

function saveAsync(moduleId, menuPropertyName, parentId, model) {
  const elementsTemp = free.mapElement(data.elements, data.metaElements);
  let _displayMode = null;
  let _numberItemPerPage = null;
  if (model) {
    _displayMode = model.displayMode;
    _numberItemPerPage = model.numberItemPerPage;
  }

  const dataToSend = {
    site: master.site,
    parentId: parentId,
    moduleId: moduleId,
    propertyName: menuPropertyName,
    elements: elementsTemp,
    state: data.data.state,
    displayMode: _displayMode,
    numberItemPerPage: _numberItemPerPage,
  };

  const promise = $http
    .post(master.getUrl('api/articles/save'), dataToSend)
    .then(function(response) {
      data.displayMode = _displayMode;
      if (response.data.isSuccess) {
        master.updateMaster(response.data.data.master);
      }
      return response.data;
    });
  _module.saveSuccess(promise, moduleId);

  return promise;
}

const initMenuAdmin = function(menuItems, menuItem) {
  menuItems.push({
    routePath: master.getInternalPath(
      '/administration/' + menuItem.routePathWithoutHomePage
    ),
    title: menuItem.title,
    moduleId: menuItem.routeDatas.moduleId,
    module: menuItem.moduleName,
    icon: menuItem.icon,
    state: menuItem.state,
  });
};

function addAsync(propertyName) {
  const elements = [
    {
      type: 'h1',
      property: 'Title',
      label: 'Titre',
      data: 'Titre de la page',
    },
    {
      type: 'p',
      property: 'Paragraphe',
      label: 'Text',
      data: 'Paragraphe de la page',
    },
  ];
  init(elements);
  return saveAsync(null, propertyName);
}

function getFirstImage(element) {
  if (element) {
    const elements = element.childs;
    if (elements) {
      for (var i = 0; i < elements.length; i++) {
        const elem = elements[i];
        if (elem.type === 'file') {
          const image = _.find(elem.data, function(o) {
            return o.displayType === 'image';
          });
          if (image) {
            return image;
          }
        }
      }
    }
  }
  const empty = {
    thumbnailUrl: '/Content/images/no-image.jpg',
    name: 'No image',
  };
  return empty;
}

export const news = {
  getQueryString,
  addAsync: addAsync,
  init: init,
  initMenuAdmin: initMenuAdmin,
  initAsync: initAsync,
  saveAsync: saveAsync,
  data: data,
  getModuleId: getModuleId,
  mapParent: free.mapParent,
  getTitle: free.getTitle,
  addNewsItem: addNewsItem,
  getFirstImage: getFirstImage,
  getFirstElement: free.getFirstElement,
};
