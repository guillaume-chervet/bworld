import app from '../../app.module';
import view from './mwBottom.html';

const name = 'mwBottom';

function ElementController() {
  const ctrl = this;

  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    data: '=',
  },
});

export default name;
