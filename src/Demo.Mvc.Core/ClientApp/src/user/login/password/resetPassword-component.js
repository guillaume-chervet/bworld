import app from '../../../app.module';
import { page } from '../../../shared/services/page-factory';
import { master } from '../../../shared/providers/master-provider';
import { externalLogin } from '../external/externalLogin-service';
import { login } from '../login-service';
import { toast as toastr } from '../../../shared/services/toastr-factory';
import $http from '../../../http';
import view from './resetPassword.html';

const name = 'resetPassword';

function ElementController() {
  page.setTitle('Demande ré-initialisation mot de passe');

  const vm = this;
  vm.user = {};
  vm.rules = {
    email: ['required', 'email'],
  };

  const submitAsync = function(form, dataToSend) {
    return $http
      .post(master.getUrl('Account/ResetPassword'), dataToSend)
      .then(function(response) {
        const result = response.data;
        if (result.isSuccess) {
          toastr.success(
            "Demande envoyé avec succès. Un email à été envoyé à l'adresse indiquée. Suivez les étapes indiquées dans le mail.",
            'Confirmation utilisateur'
          );
        }
        return response.data;
      });
  };

  vm.submit = function(form) {
    if (form.$valid) {
      const dataToSend = {
        email: vm.user.email,
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
