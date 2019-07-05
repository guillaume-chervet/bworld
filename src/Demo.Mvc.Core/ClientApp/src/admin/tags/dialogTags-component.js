import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { dialogTags } from './dialogTags-factory';
import { guid } from '../../shared/services/guid-factory';
import _ from 'lodash';
import view from './dialogTags.html';

import './dialogTags.css';

const name = 'dialogTags';

class Controller {
  $onInit() {
    const vm = this;
    page.setTitle('Administration tags', page.types.admin);
    vm.tags = [];
    const item = vm.data;
    Object.assign(vm.tags, _.cloneDeep(item.tags));
    const type = vm.data.type;
    vm.ok = function() {
      dialogTags.saveAsync(vm.tags, type).then(function(tags) {
        vm.close({ $value: tags });
      });
    };

    vm.cancel = function() {
      vm.dismiss();
    };

    vm.delete = function(tag) {
      tag.isDeleted = !tag.isDeleted;
    };

    vm.cannotDelete = function() {
      let count = 0;
      for (var i = 0; i < vm.tags.length; i++) {
        if (!vm.tags[i].isDeleted) {
          count += 1;
        }
      }
      return count < 2;
    };

    vm.add = function() {
      vm.tags.push({
        id: guid.guid(),
        name: '',
        isDeleted: false,
      });
    };

    return vm;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'dialogVm',
  bindings: {
    data: '<',
    resolve: '<',
    close: '&',
    dismiss: '&',
  },
});

export default name;
