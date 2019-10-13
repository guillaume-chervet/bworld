import app from '../../../app.module';
import history from '../../../history';
import { page } from '../../../shared/services/page-factory';
import view from './confirmAssociationError.html';

const name = 'confirmAssociationError';

class Controller {
  $onInit() {
    const ctrl = this;
    page.setTitle('Echec association provider');

    const searchObject = history.search();

    ctrl.page = {
      provider: searchObject.provider,
    };

    ctrl.goHome = function() {
      history.search({'dm': null, 'returnUrl': null, 'provider': null}, '/');
    };
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
