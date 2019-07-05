import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import view from './notAuthorised.html';

const name = 'notAuthorised';

class Controller {
  $onInit() {
    page.setTitle('Non authorisé');
    const ctrl = this;
    ctrl.goHome = function() {
      history.search('dm', null);
      history.path('/');
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
