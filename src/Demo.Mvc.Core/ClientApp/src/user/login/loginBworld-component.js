import app from '../../app.module';
import { master } from '../../shared/providers/master-provider';
import { audit } from '../../shared/services/audit-factory';
import { login } from './login-service';
import $http from '../../http';
import view from './loginBworld.html';

const name = 'loginBworld';

const Controller = function() {
  const vm = this;

  vm.user = {
    email: '',
    password: '',
  };

  vm.domainLoginUrl = login.domainLoginUrl;
  vm.getNavCreate = function() {
    var returnUrl = login.getReturnUrl();
    returnUrl = login.formatReturnUrl(returnUrl);
    return '/utilisateur/creation?dm=false&returnUrl=' + returnUrl;
  };

  vm.rules = {
    email: ['required', 'email'],
    password: login.rules.password,
  };

  vm.submit = function(form) {
    if (form.$valid) {
      const dataToSend = {
        email: vm.user.email,
        password: vm.user.password,
        rememberMe: true,
      };

      return $http
        .post(master.getUrl('Account/Login'), dataToSend)
        .then(function(response) {
          const result = response.data;
          if (result.isSuccess) {
            var returnUrl = login.getReturnUrl();
            window.location = returnUrl;
          } else {
            // TODO passer en camel case le retour MVC
            const errors = result.validationResult.errors;
            if (errors.invalidLogin) {
              form.uEmail.mw.setValidity(
                'EMAIL',
                false,
                'Login ou mot de passe non valide'
              );
            }
          }

          return response.data;
        });
    }
  };

  return vm;
};

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
});

export default name;
