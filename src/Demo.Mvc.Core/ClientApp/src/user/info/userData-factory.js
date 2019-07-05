import { master } from '../../shared/providers/master-provider';
import json from '../../shared/json';
import { audit } from '../../shared/services/audit-factory';
import $http from '../../http';
import q from '../../q';

import _ from 'lodash';

const _data = { datas: [] };
let isInitialized = false;
const initAsync = function() {
  const info = audit.getInfo();
  if (!isInitialized) {
    const siteId = master.site.siteId;
    return $http
      .get(
        master.getUrl(
          `api/user/data/${siteId}?cookieSessionId=${info.cookieSessionId}`
        )
      )
      .then(function(response) {
        isInitialized = true;
        if (response) {
          if (response.data.isSuccess) {
            const result = response.data.data;
            _data.datas.lenght = 0;
            _data.datas.push(...result.datas);
          }
        }
      });
  }
  const deferred = q.defer();
  deferred.resolve();
  return deferred.promise;
};

const getModuleId = function() {
  return master.getModuleId();
};

const saveAsync = function(data) {
  const siteId = master.site.siteId;
  return audit.getInfoAsync().then(function(info) {
    const dataToPost = {
      id: data.id,
      siteId: siteId,
      cookieSessionId: info.cookieSessionId,
      moduleId: getModuleId(),
      elementId: data.elementId,
      json: data.json,
      type: data.type,
      beginTicks: data.beginTicks,
      endTicks: data.endTicks,
    };
    return $http
      .post(master.getUrl('api/user/data'), dataToPost, {
        headers: {
          disableLoader: true,
        },
      })
      .then(function(response) {
        if (response) {
          const result = response.data;
          if (result.isSuccess) {
            const newId = response.data.data;
            dataToPost.id = newId;
            _data.datas.unshift(dataToPost);
            return newId;
          }
        }
        return null;
      });
  });
};

const getData = function(moduleId, elementId) {
  if (_data.datas) {
    const newData = _data.datas.find(
      d => d.moduleId === moduleId && d.elementId === elementId
    );
    if (newData) {
      const cloneData = _.cloneDeep(newData);
      cloneData.json = json.parse(cloneData.json);
      return cloneData;
    }
  }
  return null;
};

export const userData = {
  initAsync,
  saveAsync,
  getData,
};
