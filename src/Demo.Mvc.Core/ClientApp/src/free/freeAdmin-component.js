import app from '../app.module';
import { page } from '../shared/services/page-factory';
import { free } from './free-factory';
import { freeInit, freeOnChange } from './free-actions';
import { master } from '../shared/providers/master-provider';
import view from './free_admin.html';

import redux from '../redux';

const name = 'freeAdmin';

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    const vm = this;
    const title = free.getTitle(free.data.elements);
    page.setTitle(title, page.types.admin);

    vm.element = free.mapParent({ type: 'div', childs: free.elements });

    vm.metaElement = free.mapParent({
      type: 'div',
      childs: free.data.metaElements,
    });

    vm.data = free.data.data;

    vm.icons = [
      'fa fa-file-o',
      'glyphicon glyphicon-envelope',
      'fa fa-home',
      'fa fa-address-book',
      'fa fa-bicycle',
      'fa fa-shower',
      'fa fa-bath',
      'fa fa-coffee',
      'fa fa-history',
      'fa fa-coffee',
      'fa fa-shopping-cart',
      'fa fa-shield',
      'fa fa-book',
      'fa-address-book',
      'fa-car',
      'fa-exclamation',
      'fa-diamond',
      'fa-child',
      'fa-flash',
    ];

    const moduleId = master.getModuleId();
    vm.submit = () => {
      if (free.isUploading(vm.element.childs)) {
        return;
      }
      free.saveAsync(moduleId);
    };

    vm.delete = () => {
      if (free.isUploading(vm.element.childs)) {
        return;
      }
      module.deleteAsync(moduleId);
    };

    vm.isButtonDisabled = () => {
      return free.isUploading(vm.element.childs);
    };

    const dispatch = redux.getDispatch();
    dispatch(freeInit(vm));
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { free: state.free };
  }
  mapThisToProps() {
    return {
      onChange: (e) => redux.getDispatch()(freeOnChange(e)),
    };
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
