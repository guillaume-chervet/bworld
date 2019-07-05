import q from '../../q';
import { master } from '../../shared/providers/master-provider';
import { userNotification } from './userNotification-factory';
import { toast as toastr } from '../../shared/services/toastr-factory';
import $http from '../../http';

import redux from '../../redux';
import { userInit, userUpdate, userLogOff } from '../user-actions';

let isInitialized = false;
const initAsync = function() {
  if (!isInitialized) {
    const siteId = master.site.siteId;
    const promiseUser = $http
      .get(master.getUrl('api/user/getinfo/' + siteId))
      .then(function(response) {
        isInitialized = true;
        if (response) {
          if (response.data.isSuccess) {
            userNotification.initAsync();
            const userInfo = response.data.data;
            if (userInfo) {
              const dispatch = redux.getDispatch();
              dispatch(userInit(userInfo));
            }
          }
        }
      });
    const callback = () => {
      const state = redux.getState();
      return state.user.user.id;
    };

    return promiseUser.then(callback);
  }
  const state = redux.getState();
  const deferred = q.defer();
  deferred.resolve(state.user.user.id);
  return deferred.promise;
};

const deleteUserLoginAsync = function(provider, logins) {
  var isConfirm = window.confirm(
    'Etes-vous sûr de vouloir supprimer ce fournisseur?'
  );
  if (!isConfirm) {
    return null;
  }
  return $http
    .post(
      master.getUrl('api/user/deleteuserlogin'),
      {
        provider: provider,
      },
      {
        headers: {
          method: 'DELETE',
        },
      }
    )
    .then(function(response) {
      if (response) {
        if (response.data.isSuccess) {
          logins.splice(logins.indexOf(provider), 1);
          toastr.success(
            'Suppression effectuée avec succès.',
            'Suppression fournisseur'
          );
        }
      }
    });
};

const logOffAsync = function() {
  return $http
    .get(master.getUrl('api/user/logoff'), {
      headers: {
        loaderMessage: 'Déconnexion en cours...',
      },
    })
    .then(function(response) {
      if (response) {
        if (response.data.isSuccess) {
          const dispatch = redux.getDispatch();
          dispatch(userLogOff());
        }
      }
    });
};

function updatePerso(data) {
  const dispatch = redux.getDispatch();
  dispatch(userUpdate({ ...data, type: 'user' }));
}

function updateEmail(data) {
  const dispatch = redux.getDispatch();
  dispatch(userUpdate({ ...data, type: 'mail' }));
}

export const user = {
  updatePerso: updatePerso,
  updateEmail: updateEmail,
  initAsync: initAsync,
  logOffAsync: logOffAsync,
  deleteUserLoginAsync: deleteUserLoginAsync,
};
