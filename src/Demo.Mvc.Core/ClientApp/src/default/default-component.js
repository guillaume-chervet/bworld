import app from '../app.module';
import { page } from '../shared/services/page-factory';
import view from './default.html';

const name = 'default';

class Controller {
  $onInit() {
    page.setTitle('Pas de page configurée');
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
});

export default name;
