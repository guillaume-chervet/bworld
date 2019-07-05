import app from '../../../app.module';
import redux from '../../../redux';
import { page } from '../../../shared/services/page-factory';
import { notificationItem } from './notificationItem-factory';
import { in18Util } from '../../../shared/services/in18Util-factory';
import { free } from '../../../free/free-factory';
import { adminUser } from '../../users/adminUser-factory';
import { master } from '../../../shared/providers/master-provider';
import { dialogTags } from '../../tags/dialogTags-factory';
import _ from 'lodash';
import view from './notificationItem.html';

function compile(template, model) {
  if (!model) {
    return template;
  }
  for (var key in model) {
    if (model.hasOwnProperty(key)) {
      const find = `§model.${key}§`;
      const re = new RegExp(find, 'g');
      template = template.replace(re, model[key]);
    }
  }
  return template;
}

function compileElement(elements, model) {
  if (!elements) {
    return;
  }
  elements.forEach(function(element) {
    if (
      element.type === 'h1' ||
      element.type === 'h2' ||
      element.type === 'p'
    ) {
      element.data = compile(element.data, model);
    } else {
      if (element.childs) {
        compileElement(element.childs);
      }
    }
  });
}

const name = 'notificationItem';

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
    const user = vm.user;
    page.setTitle('Notifications utilisateurs', page.types.admin);

    vm.data = adminUser.data;
    vm.getTagInfo = dialogTags.getTagInfo;
    vm.display = in18Util.display;
    vm.selects = {};

    vm.data.users.forEach(function(user) {
      vm.selects[user.siteUserId] = false;
    }, this);

    const parentsJson = {
      type: 'div',
      childs: notificationItem.data.elements,
    };

    vm.element = free.mapParent(parentsJson);

    vm.publish = function() {
      const siteUserIds = [];
      for (var propertyName in vm.selects) {
        if (vm.selects[propertyName]) {
          siteUserIds.push(propertyName);
        }
      }
      notificationItem.sendAsync({
        elements: notificationItem.data.elements,
        siteUserIds: siteUserIds,
      });
      vm.state = 'publish';
    };

    const model = {
      UserNameSender: user.userName,
      UserName: 'Guillaume Chervet',
      SiteUrl: master.getUrl(''),
    };

    vm.state = 'edit';
    vm.preview = function() {
      vm.elementPreview = _.cloneDeep(vm.element);
      vm.state = 'preview';
      compileElement(vm.elementPreview.childs, model);
    };

    return vm;
  }

  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { user: state.user };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
