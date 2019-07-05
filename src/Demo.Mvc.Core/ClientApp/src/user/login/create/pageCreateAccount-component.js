import app from '../../../app.module';
import { page } from '../../../shared/services/page-factory';
import { externalLogin } from '../external/externalLogin-service';
import view from './pageCreateAccount.html';

const name = 'pageCreateAccount';

class Controller {
  $onInit() {
    page.setTitle('Création compte utilisateur');
    externalLogin.init();
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
