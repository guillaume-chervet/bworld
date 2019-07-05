import app from '../../app.module';
import view from './youtube.html';

const name = 'elementYoutube';

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
