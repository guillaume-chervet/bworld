import history from '../../history';
import _module from '../../adminSuper/modules/module-factory';
import { news } from '../news/news-factory';
import { urlHelper } from '../../shared/services/urlHelper-factory';
import { master } from '../../shared/providers/master-provider';
import { menu } from '../../shared/menu/menu-factory';
import { breadcrumb } from '../../breadcrumb/breadcrumb-factory';
import { free } from '../../free/free-factory';
import $http from '../../http';

import _ from 'lodash';

const data = free.createData();
const elements = data.elements;

const reInit = function() {
  elements.length = 0;
  const titre = {
    type: 'h1',
    property: 'Title',
    label: 'Titre principal',
    data: '',
  };
  elements.push(titre);
  const paragraphe = {
    type: 'p',
    property: 'Paragraphe',
    label: 'Description',
    data: '',
  };
  elements.push(paragraphe);

  const metaElements = data.metaElements;
  metaElements.length = 0;
  const metaDescription = {
    type: 'metaDescription',
    property: 'MetaDescription',
    label: 'Description qui apparaît dans les moteurs de recherche',
    data: '',
  };
  metaElements.push(metaDescription);
};

reInit();

const getModuleId = function() {
  return master.getModuleId();
};

const init = function(newElements) {
  elements.length = 0;
  for (var i = 0; i < newElements.length; i++) {
    elements.push(newElements[i]);
  }
};

const initAsync = function() {
  const siteId = master.site.siteId;
  const moduleId = getModuleId();

  const search = history.search();
  const searchClone = _.cloneDeep(search);
  searchClone.dm = undefined;
  const queryString = news.getQueryString(searchClone);

  return $http
    .get(master.getUrl(`api/articles/item/get/${siteId}/${moduleId}`))
    .then(function(response) {
      if (response) {
        const result = response.data.data;

        free.initData(result, data);
        //comment.init(result.comment);
        const items = [];
        const isAdmin = breadcrumb.isAdmin();

        let urlNews = `/articles/${
          result.parentModuleId
        }/${urlHelper.normaliseUrl(result.parentTitle)}${queryString}`;
        const title = free.getTitle(result.elements);
        let urlNewsItem = `/articles/item/${
          result.moduleId
        }/${urlHelper.normaliseUrl(title)}${queryString}`;
        if (menu.isPrivate()) {
          urlNews = `/privee${urlNews}`;
          urlNewsItem = `/privee${urlNewsItem}`;
        }

        let mainItem = null;
        let isParentHome = null;
        if (isAdmin) {
          items.push(breadcrumb.getAdminItem());
          urlNews = `/administration${urlNews}`;
          urlNewsItem = `/administration${urlNewsItem}`;
        } else {
          if (menu.isPrivate()) {
            mainItem = breadcrumb.getPrivateItem();
            isParentHome =
              result.parentModuleId ===
              menu.getMainMenuItem('privateMenuItems').moduleId;
          } else {
            mainItem = breadcrumb.getMainItem();
            isParentHome =
              result.parentModuleId === menu.getMainMenuItem().moduleId;
          }
          if (isParentHome) {
            mainItem.url += queryString;
          }
          items.push(mainItem);
        }
        // Si ce n'est pas la homepage on l'ajoute
        if (!isParentHome) {
          items.push({
            url: master.getInternalPath(urlNews),
            title: result.parentTitle,
            active: false,
            module: 'News',
          });
        }

        data.urlNews = urlNews;
        items.push({
          url: master.getInternalPath(urlNewsItem),
          title: title,
          active: true,
          module: 'NewsItem',
        });

        breadcrumb.setItems(items);
      }
    });
};

function saveAsync(moduleId, menuPropertyName, parentId) {
  const elementsTemp = free.mapElement(elements, data.metaElements);
  const dataToSend = {
    site: master.site,
    parentId: parentId,
    moduleId: moduleId,
    propertyName: menuPropertyName,
    elements: elementsTemp,
    tags: data.data.tags,
    state: data.data.state,
    isDisplayAuthor: data.data.isDisplayAuthor,
    isDisplaySocial: data.data.isDisplaySocial,
  };
  const promise = $http
    .post(master.getUrl('api/articles/item/save'), dataToSend)
    .then(function(response) {
      if (response.data.isSuccess) {
        master.updateMaster(response.data.data.master);
        if (response.data.data.newsItem) {
          return news.addNewsItem(response.data.data.newsItem);
        }
      }
      return null;
    });

  return _module.displaySaveResult(promise, moduleId);
}

const initMenuAdmin = function(menuItems, menuItem) {
  menuItems.push({
    routePath: `/administration/${menuItem.routePathWithoutHomePage}`,
    title: menuItem.title,
    moduleId: menuItem.routeDatas.moduleId,
    icon: menuItem.icon,
    state: menuItem.state,
  });
};

export const newsItem = {
  init,
  initMenuAdmin,
  initAsync,
  saveAsync,
  isUploading: free.isUploading,
  data,
  getModuleId,
  mapParent: free.mapParent,
  getTitle: free.getTitle,
  reInit,
};
