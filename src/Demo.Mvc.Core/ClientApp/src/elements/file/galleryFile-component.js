import app from '../../app.module';
import { service as fileElementService } from './elementFile-factory';
import { menuAdmin } from '../../admin/menu/menuAdmin-factory';
import { service as linkService } from '../link/elementLink-factory';
import view from './gallery.html';

const name = 'galleryFile';

class Controller {
  $onInit() {
    const ctrl = this;
    ctrl.getPath = function(data) {
      const url = linkService.getPath(data);
      return url;
    };
    ctrl.open = fileElementService.open;
    ctrl.getClass = fileElementService.getClass;
    ctrl.getAlt = fileElementService.getAlt;

    ctrl.up = function(element, file) {
      menuAdmin.up(file, element.data);
    };

    ctrl.down = function(element, file) {
      menuAdmin.down(file, element.data);
    };

    ctrl.canUp = function(element, file) {
      return menuAdmin.canUp(file, element.data);
    };

    ctrl.canDown = function(element, file) {
      return menuAdmin.canDown(file, element.data);
    };

    ctrl.destroy = function(element, file) {
      menuAdmin.deleteElement(file, element.data);
    };
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    fileTemplate: '<',
  },
});

export default name;
