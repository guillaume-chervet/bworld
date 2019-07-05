import app from '../../../app.module';
import history from '../../../history';
import { master } from '../../../shared/providers/master-provider';
import { page } from '../../../shared/services/page-factory';
import { dialogAdd } from './dialogAdd-factory';
import _ from 'lodash';
import view from './dialogAdd.html';

const name = 'dialogAdd';

class Controller {
  $onInit() {
    const ctrl = this;
    page.setTitle('Administration ajout module', page.types.admin);
    ctrl.item = ctrl.data;
    ctrl.model = {};

    ctrl.ok = function() {
      const module = _.find(ctrl.item.modules, function(o) {
        return o.id === ctrl.model.selected;
      });
      dialogAdd
        .addAsync(module, ctrl.item.propertyName)
        .then(function(response) {
          ctrl.close({ $value: ctrl.item });
          let prefix = '/administration';
          if (ctrl.item.mode === 'private') {
            prefix += '/privee';
          }
          const path = master.concatUrl(prefix, response.data.url);
          history.path(path);
        });
    };

    ctrl.cancel = function() {
      ctrl.dismiss();
    };
    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    data: '<',
    close: '&',
    dismiss: '&',
  },
});

export default name;
