import app from '../../app.module';
import history from '../../history';
import redux from '../../redux';
import { user } from '../info/user-factory';
import { login } from '../login/login-service';
import view from './associationLogin.html';

const name = 'associationLogin';

const hasLogin = function(logins, provider) {
  for (var j = 0; j < logins.length; j++) {
    if (logins[j] === provider) {
      return true;
    }
  }
  return false;
};

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    const ctrl = this;
    ctrl.hasLogin = provider => hasLogin(ctrl.logins, provider);
    ctrl.deleteUserLogin = function(provider) {
      user.deleteUserLoginAsync(provider);
    };

    // TODO a réaliser en dynamique
    ctrl.returnUrl = login.getFullBaseUrl() + history.path();
    ctrl.postUrl = login.getPostAssociationUrl();
    return this;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { logins: state.user.logins };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
