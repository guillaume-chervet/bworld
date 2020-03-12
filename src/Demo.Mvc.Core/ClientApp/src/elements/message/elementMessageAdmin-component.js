import app from '../../app.module';
import view from './message_admin.html';

const name = 'elementMessageAdmin';
class Controller {
  $onInit() {
    const vm = this;

    vm.add = function() {
      if (!vm.element.data) {
        vm.element.data = [];
      }

      vm.element.data.subjects.push({
        title: '',
      });
    };

    vm.delete = function(element) {
      const childs = vm.element.data;
      while (childs.indexOf(element) !== -1) {
        childs.splice(childs.indexOf(element), 1);
      }
    };

    return vm;
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
