import { free } from '../../../free/free-factory';
import { service as elementService } from '../../../elements/element-factory';
import _module from '../../../adminSuper/modules/module-factory';
import { urlHelper } from '../../../shared/services/urlHelper-factory';
import { breadcrumb } from '../../../breadcrumb/breadcrumb-factory';
import { master } from '../../../shared/providers/master-provider';
import { notifications } from '../hub/notifications-factory';
import $http from '../../../http';

const model = {
  tags: [],
};

const _elements = [
  {
    type: 'h1',
    property: 'Title',
    label: 'Titre',
    data: '§model.UserNameSender§ vous invite à vous inscrire sur le site',
  },
  {
    type: 'p',
    property: 'Mail',
    label: 'Mail',
    data:
      'Bonjour §model.UserName§, <br /><br />' +
      '§model.UserNameSender§ vous invite à vous inscrire sur le site: <br />' +
      '<a href="§model.SiteUrl§">§model.SiteUrl§</a><br /><br />' +
      'Cordialement,<br />' +
      '§model.UserNameSender§',
  },
];

const data = free.createData();

const sendAsync = function(notification, menuPropertyName, parentId) {
  const siteId = master.site.siteId;
  const elementsTemp = elementService.mapElement(notification.elements);
  const moduleId = getModuleId();
  const dataToSend = {
    site: master.site,
    parentId: parentId,
    moduleId: moduleId,
    propertyName: menuPropertyName,
    elements: elementsTemp,
    isDraft: data.data.isDraft,
  };

  const postData = {
    siteId: siteId,
    siteUserIds: notification.siteUserIds,
    data: dataToSend,
  };

  return $http
    .post(master.getUrl('api/notifications/item/send'), postData)
    .then(function(response) {
      return null;
    });
};

function saveAsync(moduleId, menuPropertyName, parentId) {
  const elementsTemp = free.mapElement(_elements, null);

  const dataToSend = {
    site: master.site,
    parentId: parentId,
    moduleId: moduleId,
    propertyName: menuPropertyName,
    elements: elementsTemp,
    isDraft: data.data.isDraft,
  };

  const promise = $http
    .post(master.getUrl('api/notifications/item/save'), dataToSend)
    .then(function(response) {
      if (response.data.isSuccess) {
        if (response.data.data.newsItem) {
          return notifications.addItem(response.data.data.newsItem);
        }
      }
      return null;
    });

  return _module.displaySaveResult(promise, moduleId);
}

const getModuleId = function() {
  return master.getModuleId();
};

const initAsync = function() {
  const siteId = master.site.siteId;
  const moduleId = getModuleId();

  const queryString = notifications.getQueryString();

  return $http
    .get(master.getUrl(`api/notifications/item/get/${siteId}/${moduleId}`))
    .then(function(response) {
      if (response) {
        const result = response.data.data;
        free.initData(result, data);
        const items = [];
        const isAdmin = breadcrumb.isAdmin();

        let urlNews = `/notifications${queryString}`;
        const title = free.getTitle(result.elements);
        let urlNewsItem = `/administration/notifications/item/${
          result.moduleId
        }/${urlHelper.normaliseUrl(title)}${queryString}`;
        if (isAdmin) {
          items.push(breadcrumb.getAdminItem());
          urlNews = `/administration${urlNews}`;
          urlNewsItem = `/administration${urlNewsItem}`;
        } else {
          items.push(breadcrumb.getMainItem());
        }
        // Si ce n'est pas la homepage on l'ajoute
        items.push({
          url: urlNews,
          title: 'Notifications',
          active: false,
          module: 'Notifications',
        });

        data.urlNews = urlNews;
        items.push({
          url: urlNewsItem,
          title: 'Notification',
          active: true,
          module: 'NotificationsItem',
        });

        breadcrumb.setItems(items);
      }
    });
};

export const notificationItem = {
  saveAsync: saveAsync,
  initAsync: initAsync,
  sendAsync: sendAsync,
  model: model,
  data: data,
};
