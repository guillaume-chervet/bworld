import _ from 'lodash';

import { master } from '../../shared/providers/master-provider';
import { user } from './user-factory';
import { toast as toastr } from '../../shared/services/toastr-factory';
import $http from '../../http';
import redux from '../../redux';

const userModel = {};
const initAsync = function() {
  return $http.get(master.getUrl('api/user/getuser')).then(function(response) {
    if (response) {
      if (response.data.isSuccess) {
        const result = response.data.data;
        const state = redux.getState();
        const user = state.user.user;
        Object.assign(userModel, _.cloneDeep(user));
        userModel.sites = [];
        Object.assign(userModel.sites, _.cloneDeep(result.getSites));
      }
    }
  });
};
const saveEmailAsync = function(userTemp, form) {
  const dataToPost = {
    userId: userTemp.id,
    email: userTemp.email,
  };
  return $http
    .post(master.getUrl('api/user/save'), dataToPost)
    .then(function(response) {
      if (response) {
        const result = response.data;
        if (result.isSuccess) {
          user.updateEmail(dataToPost);
          userModel.email = userTemp.email;
          toastr.success(
            'Un email de confirmation vous à été envoyé.',
            'Sauvegarde succès'
          );
        } else {
          // TODO passer en camel case le retour MVC
          const errors = result.validationResult.errors;
          if (errors.emaiL_ALREADY_EXIST) {
            form.uEmail.mw.setValidity(
              'EMAIL_ALREADY_EXIST',
              false,
              'Cet email est déjà utilisé.'
            );
          }
        }
      }
      return response;
    });
};

const savePersoAsync = function(userTemp) {
  const dataToPost = {
    userId: userTemp.id,
    firstName: userTemp.firstName,
    lastName: userTemp.lastName,
    authorUrl: userTemp.authorUrl,
  };
  return $http
    .post(master.getUrl('api/user/save'), dataToPost)
    .then(function(response) {
      if (response) {
        user.updatePerso(dataToPost);
        userModel.firstName = userTemp.firstName;
        userModel.lastName = userTemp.lastName;
        userModel.authorUrl = userTemp.authorUrl;
        toastr.success(
          'Sauvegarde de vos informations utilisateur effectuée avec succès.',
          'Sauvegarde succès'
        );
      }
    });
};

export const manageUser = {
  savePersoAsync: savePersoAsync,
  saveEmailAsync: saveEmailAsync,
  initAsync: initAsync,
  user: userModel,
};
