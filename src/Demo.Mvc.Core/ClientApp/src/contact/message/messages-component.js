import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import { messages } from './messages-factory';
import { breadcrumb } from '../../breadcrumb/breadcrumb-factory';
import view from './messages.html';

const name = 'messages';

class Controller {
  $onInit() {
    page.setTitle('Messages', page.types.admin);
    var ctrl = this;
    const breadcrumbs = breadcrumb.getItems();
    breadcrumbs[breadcrumbs.length - 1].module = 'Contact';

    const data = messages.data;

    ctrl.messages = data.messages;

    ctrl.goChat = function(chat) {
      history.path(chat.url);
    };

    ctrl.data = {
      hasPrevious: function() {
        return data.numberPrevious > 0;
      },
      urlPrevious: data.urlPrevious,
      numberPrevious: data.numberPrevious,
      numberNext: data.numberNext,
      hasNext: function() {
        return data.numberNext > 0;
      },
      urlNext: data.urlNext,
      hasPreviousOrNext: function() {
        return data.numberNext > 0 || data.numberPrevious > 0;
      },
    };
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
