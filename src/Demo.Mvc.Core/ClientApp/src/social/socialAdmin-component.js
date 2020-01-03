import _ from 'lodash';

import app from '../app.module';
import { module } from '../adminSuper/modules/module-factory';
import { master } from '../shared/providers/master-provider';
import { page } from '../shared/services/page-factory';
import { social } from './social-factory';
import view from './social_admin.html';

const name = 'socialAdmin';

class Controller {
  $onInit() {
    const ctrl = this;
    page.setTitle(social.data.title, page.types.admin);
    const moduleId = master.getModuleId();
    const _model = {};
    Object.assign(_model, _.cloneDeep(social.data));
    ctrl.model = _model;
    ctrl.rules = {
      url: ['required', 'url'],
      email: ['required', 'email'],
      phone: ['required', 'phone'],
      title: [],
      description: [],
      isAlwaysVisible: [],
      socials: ['required'],
    };
    ctrl.submit = function(form) {
      if (form.$valid) {
        social.saveAsync(moduleId, null, null, _model);
      }
    };
    ctrl.delete = function() {
      module.deleteAsync(moduleId);
    };
    return ctrl;
  }
}

app.component(name, {
  template: view ,
  controller: [Controller],
});

export default name;
