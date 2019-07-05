import { toast as toastr } from '../../shared/services/toastr-factory';
import { master } from '../../shared/providers/master-provider';
import { breadcrumb } from '../../breadcrumb/breadcrumb-factory';
import _ from 'lodash';
import $http from '../../http';
import $q from '../../q';
import $window from '../../window';

const _defaultUser = {
  lastName: '',
  firstName: '',
  birthdate: '',
  mobilePhone: '',
  phone: '',
  mail: '',
  roles: [],
  civility: null,
  comments: '',
  tags: [],
};

const data = {
  users: [],
  user: {},
};

const initAsync = function() {
  const siteId = master.site.siteId;
  return $http
    .get(master.getUrl('api/admin/user/list/' + siteId))
    .then(function(response) {
      if (response && response.data.data) {
        data.users.length = 0;
        const users = response.data.data.users;
        for (var i = 0; i < users.length; i++) {
          const user = users[i];
          data.users.push(user);
        }
      }
    });
};

const initUserAsync = function(siteUserId) {
  const items = [];
  items.push(breadcrumb.getAdminItem());
  items.push({
    url: '/administration/utilisateurs',
    title: 'Utilisateurs',
    active: false,
    module: 'User',
  });
  items.push({
    url: '',
    title: 'Edition',
    active: true,
  });
  breadcrumb.setItems(items);

  if (siteUserId) {
    const siteId = master.site.siteId;
    return $http
      .get(master.getUrl('api/admin/user/' + siteId + '/' + siteUserId))
      .then(function(response) {
        if (response && response.data.data) {
          const user = response.data.data;
          Object.assign(data.user, _.cloneDeep(user));
        }
      });
  } else {
    const deferred = $q.defer();
    const promise = deferred.promise;
    Object.assign(data.user, _.cloneDeep(_defaultUser));
    deferred.resolve(data.user);
    return promise;
  }
};

function saveAsync(user) {
  const siteId = master.site.siteId;

  const postData = {
    siteUserId: user.siteUserId,
    siteId: siteId,
    mail: user.mail,
    isEmailNotif: user.isEmailNotif,
    firstName: user.firstName,
    lastName: user.lastName,
    birthdate: user.birthdate,
    roles: user.roles,
    civility: user.civility,
    comments: user.comments,
    tags: user.tags,
  };

  return $http.post(master.getUrl('api/admin/user/save'), postData);
}

function sendInvitationAsync(siteUserId) {
  const siteId = master.site.siteId;

  const postData = {
    siteUserId: siteUserId,
    siteId: siteId,
  };

  return $http.post(master.getUrl('api/admin/user/sendInvitation'), postData);
}

function removeAsync(user) {
  const isConfirm = $window.confirm(
    'Etes-vous sûr de vouloir supprimer le role administrateur à cet utilisateur?'
  );
  if (!isConfirm) {
    return null;
  }

  const siteId = master.site.siteId;
  const dataToPost = {
    siteId: siteId,
    userId: user.id,
  };

  return $http
    .post(master.getUrl('api/admin/user/remove'), dataToPost)
    .then(function() {
      const users = data.users;
      const index = users.indexOf(user);
      users.splice(index, 1);
      toastr.success(
        'Supression effectuée avec succès.',
        'Supression role administrateur'
      );
    });
}

export const adminUser = {
  initAsync: initAsync,
  initUserAsync: initUserAsync,
  removeAsync: removeAsync,
  saveAsync: saveAsync,
  sendInvitationAsync: sendInvitationAsync,
  data: data,
};
