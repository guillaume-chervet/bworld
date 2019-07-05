import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import view from './superAdministration.html';

const name = 'superAdministration';

class Controller {
  $onInit() {
    page.setTitle('Accueil', page.types.superAdmin);
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
