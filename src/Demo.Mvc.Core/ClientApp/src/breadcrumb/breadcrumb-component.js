import app from '../app.module';
import { master } from '../shared/providers/master-provider';
import { getIcon } from '../shared/icons';
import { breadcrumb } from './breadcrumb-factory';
import './breadcrumb.css';
import view from './breadcrumb.html';
import redux from "../redux";

const name = 'breadcrumb';

class Controller  {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
        this.mapStateToThis,
        this.mapThisToProps
    )(this);
  }
  $onInit() {
    const vm = this;
    vm.items = breadcrumb.getItemsClean(vm.master.path, vm.master, vm.master.routeCurrentModuleId);
    vm.isVisible = () => breadcrumb.isVisibleClean(vm.master, vm.master.path);
    vm.ldJson = breadcrumb.getLdJsonClean(vm.items);
    vm.getIcon = getIcon;
    vm.getInternalPath = master.getInternalPath;
    return vm;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { master: state.master };
  }
  mapThisToProps() {
    return {};
  }
 
};

app.component(name, {
  template: view,
  controller: [Controller],
});

export default name;
