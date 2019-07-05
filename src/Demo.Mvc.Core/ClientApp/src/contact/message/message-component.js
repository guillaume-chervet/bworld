import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import redux from '../../redux';
import { master } from '../../shared/providers/master-provider';
import { menu } from '../../shared/menu/menu-factory';
import { message as messageService } from './message-factory';
import view from './message.html';

const name = 'message';

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    page.setTitle('Message', page.types.admin);
    const ctrl = this;
    ctrl.model = {
      message: '',
    };
    ctrl.rules = {
      message: ['required'],
    };
    ctrl.chat = messageService.data.chat;

    ctrl.goChat = function(chat) {
      history.path(chat.url);
    };
    ctrl.messageSended = false;
    let _form = null;
    ctrl.initMessage = function() {
      ctrl.model.message = '';
      ctrl.messageSended = false;
      _form.$setPristine();
    };
    ctrl.submit = function(form) {
      _form = form;
      if (_form.$valid) {
        var type = 'Reply';
        const from = {
          id: ctrl.user.id,
          type: 1,
        };

        const messageToSend = {
          message: ctrl.model.message,
          moduleId: master.site.siteId,
        };

        let source = 'Admin';
        if (!menu.isAdmin()) {
          source = 'User';
        }

        const data = {
          from: from,
          type: type,
          source: source,
          chatId: messageService.data.chat.id,
          messageJson: JSON.stringify(messageToSend),
        };

        messageService.sendMessageAsync(data).then(function() {
          ctrl.messageSended = true;
        });
      }
    };

    return ctrl;
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
  bindings: {},
});

export default name;
