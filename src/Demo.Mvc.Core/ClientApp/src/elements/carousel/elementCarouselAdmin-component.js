import app from '../../app.module';
import view from './elementCarousel_admin.html';

const name = 'elementCarouselAdmin';

class Controller {
  $onInit() {
    const ctrl = this;
    return ctrl;
  }
}
app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;
