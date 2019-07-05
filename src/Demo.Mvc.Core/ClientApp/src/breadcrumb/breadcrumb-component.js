import app from '../app.module';
import { master } from '../shared/providers/master-provider';
import { getIcon } from '../shared/icons';
import { breadcrumb } from './breadcrumb-factory';
import './breadcrumb.css';
import view from './breadcrumb.html';


const name = 'breadcrumb';

const Controller = function() {
  const vm = this;
  vm.items = breadcrumb.getItems();
  vm.isVisible = breadcrumb.isVisible;
  vm.ldJson = breadcrumb.getLdJson();
    vm.getIcon = getIcon;
    vm.getInternalPath = master.getInternalPath;
  return vm;
};

app.component(name, {
  template: view,
  controller: [Controller],
});

export default name;
