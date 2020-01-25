import app from '../../app.module';
import view from './dialogBing.html';

const name = 'dialogBing';

class Controller {
  $onInit() {
    return this;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
