import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import view from './confirmEmailError.html';

const name = 'confirmEmailError';

var Controller = function(page) {
  page.setTitle('Confirmation email échec');

  var ctrl = this;
  var searchObject = history.search();

  ctrl.user = {};
  if (searchObject.email) {
    ctrl.user.email = searchObject.email;
  }

  ctrl.page = {
    provider: searchObject.provider,
  };

  ctrl.goHome = function() {
    history.search('dm', null);
    history.search('email', null);
    history.path('/');
  };
  return ctrl;
};

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
