import app from '../app.module';
import view from './pageBreadcrumb.html';

const name = 'pageBreadcrumb';

class Controller {
  $onInit() {
    const ctrl = this;
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
