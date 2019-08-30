import app from '../../app.module';
import './file_admin.css';
import view from './elementFile_admin.html';

const name = 'elementFileAdmin';

function ElementController() {
  const ctrl = this;
  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    element: '=',
    mode: '<',
    onChange: '<',
  },
});

export default name;
