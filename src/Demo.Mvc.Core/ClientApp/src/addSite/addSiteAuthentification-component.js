import app from '../app.module';
import { page } from '../shared/services/page-factory';
import { addSite } from './addSite-factory';
import redux from '../redux';
import view from './addSiteAuthentification.html';

import './addSiteAuthentification.css';

const name = 'addSiteAuthentification';

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    page.setTitle('Création site authentification');
    const vm = this;
    vm.submit = function() {
      addSite.navNext();
    };
    vm.getNavConnect = function() {
      var thisUrl = addSite.getPaths().configuration.url;
      return '/utilisateur/connexion?dm=false&url=' + thisUrl;
    };
    vm.navBack = addSite.navBack;
    return vm;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { user: state.user.user };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
