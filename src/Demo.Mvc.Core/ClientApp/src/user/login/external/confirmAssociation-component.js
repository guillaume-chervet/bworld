import app from '../../../app.module';
import history from '../../../history';
import { page } from '../../../shared/services/page-factory';
import view from './confirmAssociation.html';

const name = 'confirmAssociation';

var Controller = function() {
  const ctrl = this;
  page.setTitle('Confirmation association provider');

  const searchObject = history.search();

  ctrl.page = {
    provider: searchObject.provider,
  };

  ctrl.goHome = function() {
    history.search('dm', null);
    history.search('returnUrl', null);
    history.search('provider', null);
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
