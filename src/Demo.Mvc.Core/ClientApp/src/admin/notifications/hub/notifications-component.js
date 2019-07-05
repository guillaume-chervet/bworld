import app from '../../../app.module';
import history from '../../../history';
import { page } from '../../../shared/services/page-factory';
import { notifications } from './notifications-factory';
import { notificationItem } from '../items/notificationItem-factory';
import view from './notifications.html';

const name = 'notifications';

class Controller {
  $onInit() {
    const vm = this;
    const title = notifications.getTitle(notifications.data.elements);
    page.setTitle(title);

    const moduleId = notifications.data.moduleId;

    vm.items = notifications.data.items;
    vm.getFirstImage = notifications.getFirstImage;

    const parentsJson = notifications.mapParent({
      type: 'div',
      childs: notifications.data.elements,
    });
    vm.element = parentsJson;

    const metaParentsJson = notifications.mapParent({
      type: 'div',
      childs: notifications.data.metaElements,
    });
    vm.metaElement = metaParentsJson;

    vm.data = {
      hasPrevious: function() {
        return notifications.data.hasPrevious;
      },
      urlPrevious: notifications.data.urlPrevious,
      hasNext: function() {
        return notifications.data.hasNext;
      },
      urlNext: notifications.data.urlNext,
      hasPreviousOrNext: function() {
        return notifications.data.hasPrevious || notifications.data.hasNext;
      },
      getDisplayMode: function() {
        return notifications.data.displayMode;
      },
      getNumberItemPerPage: function() {
        return notifications.data.numberItemPerPage;
      },
    };

    vm.model = {
      displayMode: notifications.data.displayMode,
    };

    vm.submitItem = function() {
      //	if (vm.newsForm.$valid) {
      /*var notification = {
                siteUserIds: [],
                elements: notificationItem.data.elements
            }*/

      notificationItem
        .saveAsync(null, 'MenuItems', moduleId)
        .then(function(item) {
          if (item) {
            history.path(item.data.editUrl);
          }
        });
    };

    vm.delete = function() {
      module.deleteAsync(moduleId);
    };
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
