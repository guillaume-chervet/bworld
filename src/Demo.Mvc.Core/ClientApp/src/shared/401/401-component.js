import app from '../../app.module';
import view from './401.html';
const name = '401';

class Controller {
  $onInit() {
    const ctrl = this;
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
});

export default name;
