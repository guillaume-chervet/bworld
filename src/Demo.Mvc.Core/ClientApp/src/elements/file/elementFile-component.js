import app from '../../app.module';
import { service as fileElementService } from './elementFile-factory';
import view from './elementFile.html';

const name = 'elementFile';

function ElementController() {
  var ctrl = this;
  ctrl.open = fileElementService.open;
  ctrl.getClass = fileElementService.getClass;

  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    element: '=',
  },
});

export default name;
