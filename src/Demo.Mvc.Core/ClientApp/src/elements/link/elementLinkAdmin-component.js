import app from '../../app.module';
import { service as elementService } from '../element-factory';
import { service as linkService } from './elementLink-factory';
import view from './link_admin.html';

const name = 'elementLinkAdmin';

class Controller {
  $onInit() {
    var ctrl = this;
    elementService.inherit(ctrl);
    ctrl.menuItems = linkService.init();
    ctrl.getPath = linkService.getPath;
    ctrl.getTitle = linkService.getTitle;
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;
