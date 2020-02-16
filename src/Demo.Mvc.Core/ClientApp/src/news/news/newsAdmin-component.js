import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import { news } from './news-factory';
import { newsItem } from '../newsItem/newsItem-factory';
import { master } from '../../shared/providers/master-provider';
import { isDeleted, isDraft } from '../../shared/itemStates';
import view from './news_admin.html';

const name = 'newsAdmin';

class Controller {
  $onInit() {
    const vm = this;
    vm.isDeleted = isDeleted;
    vm.isDraft = isDraft;
    const title = news.getTitle(news.data.elements);
    page.setTitle(title);

    const moduleId = master.getModuleId();

    vm.items = news.data.items;
    vm.getFirstImage = news.getFirstImage;

    newsItem.reInit();
    const addElement = news.mapParent({
      type: 'div',
      childs: newsItem.data.elements,
    });
    vm.addElement = addElement;

    const parentsJson = news.mapParent({
      type: 'div',
      childs: news.data.elements,
    });
    vm.element = parentsJson;

    const metaParentsJson = news.mapParent({
      type: 'div',
      childs: news.data.metaElements,
    });
    vm.metaElement = metaParentsJson;
    vm.data = {
      hasPrevious: function() {
        return news.data.hasPrevious;
      },
      urlPrevious: news.data.urlPrevious,
      hasNext: function() {
        return news.data.hasNext;
      },
      urlNext: news.data.urlNext,
      hasPreviousOrNext: function() {
        return news.data.hasPrevious || news.data.hasNext;
      },
      getDisplayMode: function() {
        return news.data.displayMode;
      },
      getNumberItemPerPage: function() {
        return news.data.numberItemPerPage;
      },
      data: news.data.data,
    };

    vm.model = {
      displayMode: news.data.displayMode,
      numberItemPerPage: news.data.numberItemPerPage,
    };

    vm.submit = function() {
      news.saveAsync(moduleId, null, null, vm.model);
    };

    vm.submitItem = function() {

      newsItem.saveAsync(null, 'MenuItems', moduleId).then(function(item) {
        if (item) {
          history.path(item.data.editUrl);
        }
      });
    };

    vm.delete = function() {
      module.deleteAsync(moduleId);
    };

    return vm;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
