import { master } from '../../shared/providers/master-provider';
import $http from '../../http';
import $window from '../../window';
import $q from '../../q';

const data = {
  logs: [],
};

const initAsync = function(filterData) {
  const dataToPost = {};
  if (filterData !== null) {
    dataToPost.level = filterData.level;
    dataToPost.filter = filterData.filter;
    dataToPost.origin = filterData.origin;
    dataToPost.beginDate = filterData.beginDate;
    dataToPost.endDate = filterData.endDate;
  }
  return $http
    .post(master.getUrl('api/logs/filter'), dataToPost, {
      headers: {
        loaderMessage: 'Recherche en cours...',
      },
    })
    .then(function(response) {
      if (response) {
        data.logs.length = 0;
        for (var i = 0; i < response.data.data.length; i++) {
          data.logs.push(response.data.data[i]);
        }
      }
    });
};

const clearAsync = function() {
  const isConfirm = $window.confirm(
    'Etes-vous sûr de vouloir supprimer toutes les logs'
  );
  if (!isConfirm) {
    const defered = $q.defered();
    defered.resolve();
    return defered.promise;
  }

  // var moduleId = master.getModuleId($route);
  return $http
    .post(master.getUrl('api/logs/clear'), {})
    .then(function(response) {
      if (response) {
        data.logs.length = 0;
      }
    });
};

export const logs = {
  initAsync,
  clearAsync,
  data,
};
