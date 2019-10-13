﻿import app from '../app.module';
import history from '../history';
import { master } from '../shared/providers/master-provider';
import { addSite } from './addSite-factory';
import view from './addSiteBreabcrumb.html';

const name = 'addSiteBreabcrumb';

class Controller {
  $onInit() {
    const vm = this;
    vm.paths = addSite.getPaths();
    vm.currentPath = addSite.getCurrentPath();
    vm.navStart = function() {
      const url = '/site/' + master.getModuleId() + '/configuration';
      history.search({'dm': false}, url);
    };
    return vm;
  }
}

app.component(name, {
  template: view ,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
