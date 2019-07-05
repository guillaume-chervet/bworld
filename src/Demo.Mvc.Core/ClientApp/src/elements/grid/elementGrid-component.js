import app from '../../app.module';
import view from './grid.html';

const name = 'elementGrid';

function ElementController() {
  var ctrl = this;
  return ctrl;
}

app.component(name, {
  template: view,
  controller: ElementController,
  bindings: {
    element: '=',
  },
});

export default name;
