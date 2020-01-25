import { toast as toastr } from '../../shared/services/toastr-factory';
import { master } from '../../shared/providers/master-provider';
import $http from '../../http';
import $window from '../../window';

const _users = [];

const initAsync = function() {
  _users.length = 0;
  return $http.get(master.getUrl('api/user/getusers')).then(function(response) {
    if (response) {
      for (var i = 0; i < response.data.data.length; i++) {
        var user = response.data.data[i];
        _users.push(user);
      }
    }
  });
};

function deleteAsync(user) {
  const isConfirm = $window.confirm(
    'Etes-vous sûr de vouloir supprimer cet élément?'
  );
  if (!isConfirm) {
    return null;
  }
  return $http
    .delete(master.getUrl('api/user/delete/' + user.id))
    .then(function() {
      var index = _users.indexOf(user);
      _users.splice(index, 1);
      toastr.success(
        'Supression effectuée avec succès.',
        'Supression utilisateur'
      );
    });
}

export const users = {
  initAsync: initAsync,
  deleteAsync: deleteAsync,
  users: _users,
};
