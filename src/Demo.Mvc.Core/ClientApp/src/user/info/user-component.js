import _ from 'lodash';

import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { manageUser } from './manageUser-service';
import { in18Util } from '../../shared/services/in18Util-factory';

import view from './user.html';

const name = 'user';
class Controller {
  $onInit() {
    var ctrl = this;
    page.setTitle('Paramètre(s) utilisateur', page.types.user);

    ctrl.rules = {
      email: ['required', 'email'],
      lastName: ['required', 'lastname'],
      firstName: ['required', 'firstname'],
      authorUrl: 'url',
    };

    var userModel = {};
    Object.assign(userModel, manageUser.user);

    ctrl.user = userModel;

    ctrl.isEditPerso = false;
    ctrl.isEditMail = false;
    ctrl.display = in18Util.display;

    ctrl.editPerso = function() {
      ctrl.isEditPerso = true;
    };

    ctrl.cancelPerso = function() {
      ctrl.isEditPerso = false;
      Object.assign(userModel, _.cloneDeep(manageUser.user));
    };

    ctrl.submitPerso = function(formPerso) {
      if (formPerso && formPerso.$valid) {
        ctrl.isEditPerso = false;
        manageUser.savePersoAsync(userModel).then(function() {
          Object.assign(userModel, _.cloneDeep(manageUser.user));
        });
      }
    };

    ctrl.submitMail = function(formEmail) {
      if (formEmail && formEmail.$valid) {
        formEmail.uEmail.mw.setValidity('EMAIL_ALREADY_EXIST', true);
        formEmail.uEmail.mw.setValidity('EMAIL_NOT_MODIFIED', true);
        if (userModel.email === manageUser.user.email) {
          formEmail.uEmail.mw.setValidity(
            'EMAIL_NOT_MODIFIED',
            false,
            "L'email n'a pas été modifié."
          );
        } else {
          manageUser
            .saveEmailAsync(userModel, formEmail)
            .then(function(response) {
              if (response) {
                var result = response.data;
                if (result.isSuccess) {
                  ctrl.isEditMail = false;
                  Object.assign(userModel, _.cloneDeep(manageUser.user));
                }
              }
            });
        }
      }
    };

    ctrl.editMail = function() {
      ctrl.isEditMail = true;
    };

    ctrl.cancelMail = function(formEmail) {
      ctrl.isEditMail = false;
      Object.assign(userModel, _.cloneDeep(manageUser.user));
      formEmail.uEmail.mw.setValidity('EMAIL_ALREADY_EXIST', true);
      formEmail.uEmail.mw.setValidity('EMAIL_NOT_MODIFIED', true);
    };

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
