import { USER_INIT, USER_UPDATE, USER_LOGOFF } from './user-types';
import _ from 'lodash';

export const initialState = {
  alerts: [],
  logins: [],
  user: {
    isAuthenticate: false,
  },
};

const updateUser = (_user, userTemp) => {
  switch (userTemp.type) {
    case 'mail': {
      _user.email = userTemp.email;
      break;
    }
    case 'user': {
      _user.userName = userTemp.firstName + ' ' + userTemp.lastName;
      _user.firstName = userTemp.firstName;
      _user.lastName = userTemp.lastName;
      _user.authorUrl = userTemp.authorUrl;
      break;
    }
  }
  return _user;
};

const logOffUser = _user => {
  _user.id = '';
  _user.userName = '';
  _user.firstName = '';
  _user.lastName = '';
  _user.authorUrl = '';
  _user.emailConfirmed = false;
  _user.email = '';
  _user.isAuthenticate = false;
  _user.isAdministrator = false;
  _user.isSuperAdministrator = false;
  return _user;
};

const user = (state = initialState, action) => {
  switch (action.type) {
    case USER_INIT: {
      const data = action.data;
      return {
        ...initUser(data),
      };
    }
    case USER_UPDATE: {
      const data = action.data;
      return {
        ...state,
        user: updateUser({ ...state.user }, data),
      };
    }
    case USER_LOGOFF: {
      return {
        ...state,
        user: logOffUser({ ...state.user }),
      };
    }
    default:
      return state;
  }
};

const initUser = function(result) {
  const userInfo = result.user;
  const user = { ...initialState.user };
  const logins = [];
  const alerts = [];
  if (userInfo) {
    user.id = userInfo.id;
    user.userName = userInfo.userName;
    user.firstName = userInfo.firstName;
    user.lastName = userInfo.lastName;
    user.authorUrl = userInfo.authorUrl;
    user.emailConfirmed = userInfo.emailConfirmed;
    user.email = userInfo.email;
    user.isAuthenticate = true;
    user.isAdministrator = false;
    user.isSuperAdministrator = false;
    user.roles = {
      isDeveloper: false,
      isPrivateUser: false,
    };

    if (userInfo.roles) {
      for (let i = 0; i < userInfo.roles.length; i++) {
        const role = userInfo.roles[i];

        if (role === 'super_administrator') {
          user.isAdministrator = true;
          user.isSuperAdministrator = true;
          user.roles.isPrivateUser = true;
          break;
        } else if (role === window.params.master.site.siteId) {
          user.isAdministrator = true;
          user.roles.isPrivateUser = true;
        } else if (
          role ===
          window.params.master.site.siteId + '_private_user'
        ) {
          user.roles.isPrivateUser = true;
        }
      }
    }

    if (userInfo.logins) {
      logins.length = 0;
      for (let j = 0; j < userInfo.logins.length; j++) {
        logins.push(userInfo.logins[j]);
      }
    }

    alerts.length = 0;
    if (
      user.isAuthenticate &&
      !user.emailConfirmed &&
      !user.isSuperAdministrator
    ) {
      alerts.push({
        type: 'danger',
        msg:
          "Vous devez valider votre adresse email. Cliquer sur le lien que vous avez reçu par email afin de le valider. Il se peut que l'mail soit présent dans vos spam.",
      });
    }
  }
  return { user, logins, alerts };
};

export default user;
