import app from '../app.module';
import view from './page.html';

const name = 'page';

class Controller {
  $onInit() {
    var ctrl = this;
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: Controller,
  transclude: {
    content: 'content',
  },
});

export default name;
