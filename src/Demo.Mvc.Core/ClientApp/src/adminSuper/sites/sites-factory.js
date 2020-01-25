import { toast as toastr } from '../../shared/services/toastr-factory';
import { master } from '../../shared/providers/master-provider';
import $http from '../../http';
import $window from '../../window';

const _sites = [];

const initAsync = function() {
  _sites.length = 0;
  return $http.get(master.getUrl('api/site/getsites')).then(function(response) {
    if (response) {
      for (var i = 0; i < response.data.data.length; i++) {
        var site = response.data.data[i];
        _sites.push(site);
      }
    }
  });
};

function deleteAsync(site) {
  const isConfirm = $window.confirm(
    'Etes-vous sûr de vouloir supprimer cet élément?'
  );
  if (!isConfirm) {
    return null;
  }
  return $http
    .delete(master.getUrl('api/site/delete/' + site.siteId))
    .then(function() {
      const index = _sites.indexOf(site);
      _sites.splice(index, 1);
      toastr.success('Supression effectuée avec succès.', 'Supression site');
    });
}

function clearCacheAsync() {
  const isConfirm = $window.confirm(
    'Etes-vous sûr de vouloir supprimer toutes les caches?'
  );
  if (!isConfirm) {
    return null;
  }
  return $http.post(master.getUrl('api/site/clearcache')).then(function() {
    toastr.success('Supression effectuée avec succès.', 'Supression cache');
  });
}

export const sites = {
  initAsync: initAsync,
  deleteAsync: deleteAsync,
  clearCacheAsync: clearCacheAsync,
  sites: _sites,
};
