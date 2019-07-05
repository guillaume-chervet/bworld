import history from '../../history';
import $window from '../../window';
import q from '../../q';
import { toast } from '../../shared/services/toastr-factory';
import { master } from '../../shared/providers/master-provider';
import $http from '../../http';

const name = 'Module';

const deleteAsync = function(moduleId, redirectUrl) {
  const isConfirm = $window.confirm(
    'Etes-vous sûr de vouloir supprimer cet élément?'
  );
  if (!isConfirm) {
    const defered = q.defer();
    defered.resolve();
    return defered.promise;
  }
  const dataToSend = {
    site: master.site,
    moduleId: moduleId,
  };
  const promise = $http
    .post(master.getUrl('api/module/delete'), dataToSend, {
      headers: {
        method: 'DELETE',
      },
    })
    .then(function(response) {
      if (response.data.isSuccess) {
        master.updateMaster(response.data.data);
      }
    });

  promise.then(function() {
    if (redirectUrl) {
      history.path(redirectUrl);
    } else {
      history.path('/administration');
    }
    toast.success('Suppression effectuée avec succès.', 'Suppression module');
  });

  return promise;
};

function saveSuccess(promise, moduleId) {
  promise.then(function(response) {
    if (moduleId) {
      toast.success('Sauvegarde effectuée avec succès.', 'Sauvegarde module');
    } else if (!response.data.url) {
      history.path('/administration');
      toast.success('Ajout du module effectué avec succès.', 'Ajout Module');
    }

    return response;
  });
}

function displaySaveResult(promise, moduleId) {
  return promise.then(function(response) {
    if (moduleId) {
      toast.success('Sauvegarde effectuée avec succès.', 'Sauvegarde');
    } else {
      toast.success('Ajout effectué avec succès.', 'Ajout');
    }
    return response;
  });
}

function saveMenuAsync(menus) {
  const dataToSend = {
    site: master.site,
  };

  if (menus) {
    for (var name in menus) {
      dataToSend[name] = menus[name];
    }
  }

  return $http
    .post(master.getUrl('api/module/menu/save'), dataToSend)
    .then(function(response) {
      if (response.data.isSuccess) {
        master.updateMaster(response.data.data);
        toast.success('Sauvegarde effectuée avec succès.', 'Sauvegarde menu');
      }
    });
}

const superAdminGetAsync = function(data) {
  const promise = $http
    .post(master.getUrl('api/module/superadminget'), data)
    .then(function(response) {
      return response.data;
    });
  return promise;
};

const superAdminSaveAsync = function(item) {
  const promise = $http
    .post(master.getUrl('api/module/superadminsave'), item)
    .then(function(response) {
      toast.success('Sauvegarde effectuée avec succès.', 'Sauvegarde module');
      return response.data;
    });
  return promise;
};

const superAdminDeleteAsync = function(moduleId) {
  const isConfirm = $window.confirm(
    'Etes-vous sûr de vouloir supprimer cet élément?'
  );
  if (!isConfirm) {
    return null;
  }
  const promise = $http
    .delete(master.getUrl('api/module/superadmindelete/' + moduleId))
    .then(function(response) {
      toast.success('Suppression effectuée avec succès.', 'Suppression module');
      return response.data;
    });
  return promise;
};

export const module = {
  deleteAsync: deleteAsync,
  saveMenuAsync: saveMenuAsync,
  saveSuccess: saveSuccess,
  superAdminGetAsync: superAdminGetAsync,
  superAdminSaveAsync: superAdminSaveAsync,
  superAdminDeleteAsync: superAdminDeleteAsync,
  displaySaveResult: displaySaveResult,
};
