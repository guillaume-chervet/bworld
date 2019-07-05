import app from '../../../app.module';
import history from '../../../history';
import { page } from '../../../shared/services/page-factory';
import { master } from '../../../shared/providers/master-provider';
import { login } from '../login-service';
import { toast as toastr } from '../../../shared/services/toastr-factory';
import $http from '../../../http';
import view from './reinitPassword.html';

const name = 'reinitPassword';

function ElementController() {
  page.setTitle('Ré-initialisation mot de passe');

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
    password: login.rules.password,
    passwordConfirm: ['required', customPassword],
  };

  vm.showPassword = function() {
    vm.passwordHided = !vm.passwordHided;
  };

  vm.hidePassword = function() {
    vm.passwordHided = !vm.passwordHided;
  };

  const submitAsync = function(form, dataToSend) {
    return $http
      .post(master.getUrl('Account/ChangePassword'), dataToSend)
      .then(function(response) {
        const result = response.data;
        if (result.isSuccess) {
          toastr.success(
            'Changement du mot de passe réalisé avec succès. Vous allez être redirigé vers la page initiale.',
            'Confirmation utilisateur'
          );
          history.path('/utilisateur/connexion');
        }

        return response.data;
      });
  };

  vm.submit = function(form) {
    if (form.$valid) {
      const search = history.search();
      const dataToSend = {
        userId: search.userId,
        token: search.token,
        password: vm.user.password,
      };
      submitAsync(form, dataToSend);
    }
  };

  return vm;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  controllerAs: 'vm',
  transclude: {
    submit: '?submit',
  },
  bindings: {},
});

export default name;
