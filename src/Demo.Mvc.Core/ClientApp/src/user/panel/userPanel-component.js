import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { manageUser } from '../info/manageUser-service';
import { userNotification } from '../info/userNotification-factory';
import view from './userPanel.html';
import { master } from '../../shared/providers/master-provider';

const name = 'userPanel';

class Controller {
  $onInit() {
    page.setTitle('Accueil', page.types.user);
    const ctrl = this;
    ctrl.user = manageUser.user;
    ctrl.notification = {
      numberUnreadMessage: userNotification.data.numberUnreadMessage,
      };
      ctrl.getInternalPath = master.getInternalPath;
    ctrl.goCreateSite = function() {
      window.location =
        'https://www.bworld.fr/site/c176c987-59b2-4a59-93bd-a54d73e3dc2e/authentification?dm=false';
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
