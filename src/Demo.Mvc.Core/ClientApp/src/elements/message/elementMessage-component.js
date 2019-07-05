import _ from 'lodash';

import app from '../../app.module';
import { message as messageService } from '../../contact/message/message-factory';
import { master } from '../../shared/providers/master-provider';
import redux from '../../redux';
import view from './message.html';

const name = 'elementMessage';

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
    vm.message = {
      title: '',
      message: '',
    };
    vm.rules = {
      email: ['required', 'email'],
      lastName: ['required', 'lastName'],
      title: ['required'],
      message: ['required'],
      firstName: ['required', 'firstName'],
      phone: ['phone'],
    };
    let _formMessage = null;
    vm.submit = function(formMessage) {
      _formMessage = formMessage;
      if (formMessage.$valid) {
        const message = vm.message;
        let messageToSend = null;
        let type = '';
        const from = {
          id: null,
          type: 1,
        };
        if (vm.user.isAuthenticate) {
          from.id = vm.user.id;
          messageToSend = {
            message: message.message,
            title: message.title,
            moduleId: master.getModuleId(),
          };
          type = 'SiteAuthenticated';
        } else {
          from.id = message.email;
          from.type = 2;
          messageToSend = {
            moduleId: master.getModuleId(),
          };
          Object.assign(messageToSend, _.cloneDeep(message));
          type = 'SiteNotAuthenticated';
        }

        const data = {
          from: from,
          to: {
            id: master.site.siteId,
            type: 0,
          },
          type: type,
          source: 'User',
          messageJson: JSON.stringify(messageToSend),
        };

        messageService.sendMessageAsync(data).then(function() {
          vm.messageSended = true;
        });
      }
    };
    vm.messageSended = false;
    vm.initMessage = function() {
      const message = vm.message;
      message.title = '';
      message.lastName = '';
      message.firstName = '';
      message.email = '';
      message.phone = '';
      message.message = '';
      _formMessage.$setPristine();
      vm.messageSended = false;
    };
    vm.getClassLabel = function(element) {
      if (element.$level <= 2) {
        return 'col-sm-3';
      }
      return 'col-sm-12 col-md-4';
    };
    vm.getClassField = function(element) {
      if (element.$level <= 2) {
        return 'col-sm-6';
      }
      return 'col-sm-12 col-md-8';
    };
    vm.getClassAction = function(element) {
      if (element.$level <= 2) {
        return 'col-sm-offset-3 col-sm-9 col-xs-offset-6 col-xs-6 mw-action';
      }
      return 'col-sm-offset-4 col-sm-9 col-md-offset-4 col-md-8 col-xs-offset-6 col-xs-6 mw-action';
    };
    return vm;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { user: state.user.user };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
  },
});

export default name;
