import app from '../../../app.module';
import history from '../../../history';
import { page } from '../../../shared/services/page-factory';
import { master } from '../../../shared/providers/master-provider';
import { externalLogin } from '../external/externalLogin-service';
import { toast as toastr } from '../../../shared/services/toastr-factory';
import $http from '../../../http';
import view from './externalLoginConfirmation.html';

const name = 'externalLoginConfirmation';

class Controller {
  $onInit() {
    externalLogin.init();
    page.setTitle('Confirmation compte externe');
    const ctrl = this;
    ctrl.user = {};
    if (externalLogin.externalLogin.email) {
      ctrl.user.email = externalLogin.externalLogin.email;
    }

    ctrl.page = {
      provider: externalLogin.externalLogin.provider,
    };

    ctrl.rules = {
      condition: [
        'required',
        {
          equal: {
            equal: true,
            message: 'Vous devez accèpter les règles de confidentialité.',
          },
        },
      ],
      email: ['required', 'email'],
      lastName: 'required',
      firstName: 'required',
    };

    const submitLoginAsync = function(form, dataToSend, returnUrl) {
      returnUrl = externalLogin.getReturnUrl(returnUrl);
      form.uEmail.mw.setValidity('EMAIL', true);
      return $http
        .post(master.getUrl('Account/ExternalLoginConfirmation'), dataToSend)
        .then(function(response) {
          const result = response.data;
          if (result.isSuccess) {
            toastr.success(
              'Confirmation réalisé avec succès. Un email de confirmation vous à été envoyé. Vous allez être redirigé vers la page initiale.',
              'Confirmation utilisateur'
            );
            window.location = returnUrl;
          } else {
            // TODO passer en camel case le retour MVC
            const errors = result.validationResult.errors;
            if (errors.email) {
              form.uEmail.mw.setValidity('EMAIL', false, errors.email.message);
            }

            if (errors.eXTERNAL_PROVIDER) {
              history.path(
                '/utilisateur/erreur-compte-externe?provider=' +
                  ctrl.page.provider +
                  '&returnUrl=' +
                  externalLogin.externalLogin.returnUrl
              );
            }
          }
          return response.data;
        });
    };

    ctrl.submit = function(form) {
      if (form.$valid) {
        const dataToSend = {
          email: ctrl.user.email,
          firstName: ctrl.user.firstName,
          lastName: ctrl.user.lastName,
          provider: ctrl.page.provider,
        };
        const returnUrl = externalLogin.externalLogin.returnUrl;
        submitLoginAsync(form, dataToSend, returnUrl);
      }
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
