import app from '../../app.module';
import { service as elementService } from '../element-factory';
import view from './metaDescription_admin.html';

const name = 'elementMetaDescriptionAdmin';

class Controller {
  $onInit() {
    var ctrl = this;
    elementService.inherit(ctrl);
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    mode: '<',
    onChange: '<',
  },
});

export default name;
