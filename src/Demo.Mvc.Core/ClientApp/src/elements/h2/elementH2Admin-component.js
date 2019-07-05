import app from '../../app.module';
import view from './h2_admin.html';

const name = 'elementH2Admin';

function ElementController() {
  var ctrl = this;
  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;
