import { master } from '../../shared/providers/master-provider';
import _ from 'lodash';

import $http from '../../http';

const data = {};

const filter = {
  date: new Date(),
};

const timeDay = 24 * 60 * 60 * 1000;

const nextAsync = function() {
  filter.date = new Date(filter.date.getTime() + timeDay);
  return initAsync();
};

const isNextDisabled = function() {
  if (filter.date.getTime() + timeDay > new Date().getTime()) {
    return true;
  }
  return false;
};

var previousAsync = function() {
  filter.date = new Date(filter.date.getTime() - timeDay);
  return initAsync();
};

var initAsync = function() {
  var dataToPost = {
    siteId: master.site.siteId,
    date: filter.date,
  };

  return $http
    .post(master.getUrl('api/stat/get'), dataToPost, {
      headers: {
        loaderMessage: 'Chargement en cours...',
      },
    })
    .then(function(response) {
      if (response) {
        if (data.stats) {
          data.stats.length = 0;
        }
        if (data.hours) {
          data.hours.length = 0;
        }
        if (data.regions) {
          data.regions.length = 0;
        }

        if (data.referrers) {
          data.referrers.length = 0;
        }

        if (data.devices) {
          data.devices.length = 0;
        }
        if (response.data.isSuccess) {
          Object.assign(data, _.cloneDeep(response.data.data));
        }
      }
    });
};

export const stats = {
  initAsync: initAsync,
  data: data,
  filter: filter,
  previousAsync: previousAsync,
  nextAsync: nextAsync,
  isNextDisabled: isNextDisabled,
};
