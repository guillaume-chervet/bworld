import app from '../app.module';
import view from './pageFullBreadcrumb.html';

const name = 'pageFullBreadcrumb';

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
