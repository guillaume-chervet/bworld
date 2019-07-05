import app from '../../app.module';
import redux from '../../redux';
import { page } from '../../shared/services/page-factory';
import view from './login.html';

const name = 'login';

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    page.setTitle('Authentification');
    const ctrl = this;
    return ctrl;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { isAuthenticate: state.user.isAuthenticate };
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
