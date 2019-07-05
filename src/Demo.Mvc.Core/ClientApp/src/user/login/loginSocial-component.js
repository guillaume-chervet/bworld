import app from '../../app.module';
import { login } from './login-service';
import view from './loginSocial.html';

const name = 'loginSocial';

class Controller {
  $onInit() {
    const ctrl = this;
    const returnUrl = login.getReturnUrl();
    ctrl.returnUrl = returnUrl;
    ctrl.postUrl = login.getPostUrl();
    ctrl.domainLoginUrl = login.domainLoginUrl;

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
