import app from '../../app.module';
import { service as elementGridElementService } from './elementGridElement-factory';
import view from './gridElement.html';

const name = 'elementGridElement';

function ElementController() {
  var ctrl = this;

  ctrl.cssClass = function(element) {
    return elementGridElementService.cssClass(element, ctrl.noClass);
  };

  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    element: '=',
    noClass: '<',
  },
});

export default name;
