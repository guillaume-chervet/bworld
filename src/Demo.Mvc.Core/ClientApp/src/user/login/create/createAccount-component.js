import app from '../../../app.module';
import { master } from '../../../shared/providers/master-provider';
import { externalLogin } from '../external/externalLogin-service';
import { login } from '../login-service';
import { toast as toastr } from '../../../shared/services/toastr-factory';
import $http from '../../../http';
import view from './createAccount.html';

const name = 'createAccount';

class Controller {
  $onInit() {
    externalLogin.init();
    const vm = this;
    vm.user = {};
    vm.passwordHided = true;

    const validatePassword = function() {
      if (vm.user.password === vm.user.passwordConfirm) {
        return { success: true, message: '' };
      }
      return {
        success: false,
        message: 'Les deux mot de passe doivent être identique.',
      };
    };
    const customPassword = {
      custom: {
        validateView: validatePassword,
        validateModel: validatePassword,
      },
    };

    vm.rules = {
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
      password: login.rules.password,
      passwordConfirm: ['required', customPassword],
      lastName: 'required',
      firstName: 'required',
    };

    vm.showPassword = function() {
      vm.passwordHided = !vm.passwordHided;
    };

    vm.hidePassword = function() {
      vm.passwordHided = !vm.passwordHided;
    };

    const submitLoginAsync = function(form, dataToSend) {
      const returnUrl = externalLogin.getReturnUrl(
        externalLogin.externalLogin.returnUrl
      );
      form.uEmail.mw.setValidity('EMAIL', true);
      return $http
        .post(master.getUrl('Account/Register'), dataToSend)
        .then(function(response) {
          const result = response.data;
          if (result.isSuccess) {
            toastr.success(
              'Creation réalisé avec succès. Un email de confirmation vous à été envoyé. Vous allez être redirigé vers la page initiale.',
              'Confirmation utilisateur'
            );
            window.location = returnUrl;
          } else {
            // TODO passer en camel case le retour MVC
            var errors = result.validationResult.errors;
            if (errors.email) {
              form.uEmail.mw.setValidity('EMAIL', false, errors.Email.Message);
            }
          }

          return response.data;
        });
    };

    vm.submit = function(form) {
      if (form.$valid) {
        const dataToSend = {
          email: vm.user.email,
          firstName: vm.user.firstName,
          lastName: vm.user.lastName,
          password: vm.user.password,
        };
        submitLoginAsync(form, dataToSend);
      }
    };

    return vm;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  transclude: {
    submit: '?submit',
  },
  bindings: {},
});

export default name;
