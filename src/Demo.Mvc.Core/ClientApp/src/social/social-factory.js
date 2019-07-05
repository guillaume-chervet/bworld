import _ from 'lodash';

import { module } from '../adminSuper/modules/module-factory';
import { master } from '../shared/providers/master-provider';
import $http from '../http';

const data = {
  url: 'https://www.facebook.com/bworld.site/',
  title: 'Facebook',
  description: 'Notre page facebook',
  isAlwaysVisible: true,
  socials: 2,
};

const getModuleId = function() {
  return master.getModuleId();
};

const initAsync = function() {
  const siteId = master.site.siteId;
  const moduleId = getModuleId();
  return $http
    .get(master.getUrl(`api/social/get/${siteId}/${moduleId}`))
    .then(function(response) {
      if (response) {
        Object.assign(data, _.cloneDeep(response.data.data.data));
      }
    });
};

function saveAsync(moduleId, menuPropertyName, parentId, model) {
  if (!model) {
    model = {
      url: 'https://www.facebook.com/bworld.site/',
      title: 'Facebook',
      description: 'Notre page facebook',
      isAlwaysVisible: true,
      socials: 2,
    };
  }
  const dataToSend = {
    site: master.site,
    parentId: parentId,
    moduleId: moduleId,
    propertyName: menuPropertyName,
    data: model,
  };

  const promise = $http
    .post(master.getUrl('api/social/save'), dataToSend)
    .then(function(response) {
      if (response.data.isSuccess) {
        Object.assign(data, _.cloneDeep(model));
        master.updateMaster(response.data.data.master);
      }
      return response.data;
    });
  module.saveSuccess(promise, moduleId);

  return promise;
}

const initMenuAdmin = function(menuItems, menuItem) {
  menuItems.push({
    routePath: `/administration/${menuItem.routePathWithoutHomePage}`,
    title: menuItem.title,
    moduleId: menuItem.routeDatas.moduleId,
    module: menuItem.moduleName,
    state: menuItem.state,
  });
};

function addAsync(propertyName) {
  return saveAsync(null, propertyName);
}

export const social = {
  addAsync: addAsync,
  initMenuAdmin: initMenuAdmin,
  initAsync: initAsync,
  saveAsync: saveAsync,
  data: data,
};
