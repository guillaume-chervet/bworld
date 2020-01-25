import history from '../../../history';
import { master } from '../../../shared/providers/master-provider';
import { urlHelper } from '../../../shared/services/urlHelper-factory';
import { free } from '../../../free/free-factory';
import { breadcrumb } from '../../../breadcrumb/breadcrumb-factory';
import $http from '../../../http';
import $route from '../../../route';

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

const getModuleId = function() {
  return master.getModuleId();
};

const init = function(newElements) {
  elements.length = 0;
  elements.push(...newElements);
};

const getQueryString = function() {
  const search = history.search();
  const index = search.index;

  let queryString = '';
  if (index) {
    queryString = '?index=' + index;
  }
  return queryString;
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

  const queryString = getQueryString();

  const title = free.getTitle(item.elements);
  const viewUrl =
    '/notifications/item/' +
    item.moduleId +
    '/' +
    urlHelper.normaliseUrl(title) +
    queryString;
  const editUrl = '/administration' + viewUrl + queryString;

  const newItem = {};

  const newsElements = [];
  const paragraph = free.getFirstParagraph(itemData.elements);
  if (paragraph) {
    newsElements.push(paragraph);
  }
  const image = free.getFirstImage(itemData.elements);
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

const addItem = function(item) {
  const mappedItem = mapItem(item);
  data.items.splice(0, 0, mappedItem);
  return mappedItem;
};

const initAsync = function() {
  const siteId = master.site.siteId;
  const moduleId = getModuleId();

  let url = 'api/notifications/' + siteId;

  const search = history.search();
  const index = search.index;
  let title = $route.current().params.title;

  if (index) {
    url = url + '/' + index;
  }

  const items = [];
  items.push(breadcrumb.getAdminItem());
  items.push({
    url: '/administration/notifications',
    title: 'Notifications',
    active: false,
    module: 'Notifications',
  });
  breadcrumb.setItems(items);

  return $http.get(master.getUrl(url)).then(function(response) {
    if (response) {
      const result = response.data.data;
      free.initData(result, data);
      data.moduleId = result.moduleId;
      data.items = [];

      if (result.getNewsItem) {
        result.getNewsItem.forEach(function(item) {
          data.items.push(mapItem(item));
        });

        const isAdmin = breadcrumb.isAdmin();
        data.hasNext = result.hasNext;

        let urlNext =
          '/notifications/' +
          moduleId +
          '/' +
          title +
          '?index=' +
          result.idNext;
        let urlPrevious = '/notifications/' + moduleId + '/' + title;
        if (isAdmin) {
          urlNext = '/administration' + urlNext;
          urlPrevious = '/administration' + urlPrevious;
        }
        data.urlNext = urlNext;
        data.hasPrevious = result.hasPrevious;

        data.urlPrevious = urlPrevious;
        if (result.idPrevious) {
          data.urlPrevious += '?index=' + result.idPrevious;
        }

        data.displayMode = result.displayMode;
        data.numberItemPerPage = result.numberItemPerPage;
      }
    }
  });
};

const initMenuAdmin = function(menuItems, menuItem) {
  menuItems.push({
    routePath: '/administration/' + menuItem.routePathWithoutHomePage,
    title: menuItem.title,
    moduleId: menuItem.routeDatas.moduleId,
    module: menuItem.moduleName,
  });
};

function addAsync() {
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
}

function getFirstImage(element) {
  const newElement = free.getFirstImage(element.childs);
  if (!newElement || newElement.data.length <= 0) {
    return {
      thumbnailUrl: '/Content/images/no-image.jpg',
      name: 'No image',
    };
  } else {
    return newElement.data[0];
  }
}

export const notifications = {
  addAsync: addAsync,
  init: init,
  initMenuAdmin: initMenuAdmin,
  initAsync: initAsync,
  data: data,
  getModuleId: getModuleId,
  mapParent: free.mapParent,
  getTitle: free.getTitle,
  addItem: addItem,
  getQueryString: getQueryString,
  getFirstImage: getFirstImage,
};
