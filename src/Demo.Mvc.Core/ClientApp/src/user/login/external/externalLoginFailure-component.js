import app from '../../../app.module';
import history from '../../../history';
import { page } from '../../../shared/services/page-factory';
import { externalLogin } from './externalLogin-service';
import view from './externalLoginFailure.html';

const name = 'externalLoginFailure';

class Controller {
  $onInit() {
    externalLogin.init();
    page.setTitle('Association compte échec');

    const ctrl = this;
    ctrl.goHome = function() {
      history.search('dm', null);
      history.path('/');
    };

    ctrl.page = {
      provider: externalLogin.externalLogin.provider,
      returnUrl: externalLogin.externalLogin.returnUrl,
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
