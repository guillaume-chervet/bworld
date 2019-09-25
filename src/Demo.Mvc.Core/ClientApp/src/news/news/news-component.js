import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import { news } from './news-factory';
import { dialogTags } from '../../admin/tags/dialogTags-factory';
import { isDraft, isDeleted } from '../../shared/itemStates';
import view from './news.html';

import './news.css';

const name = 'news';

class Controller {
  $onInit() {
    const vm = this;
    vm.isDeleted = isDeleted;
    vm.isDraft = isDraft;
    const title = news.getTitle(news.data.elements);
    page.setTitle(title);

    const items = news.data.items.reduce((accumulator, item) => {
      if (!isDraft(item)) {
        accumulator.push(item);
      }
      return accumulator;
    }, []);

    vm.items = items;
    vm.getFirstImage = news.getFirstImage;

    const selectedTags = () => {
      const search = history.search();

      const selectedTags = [];
      const tags = search.tags;
      if (tags) {
        if (Array.isArray(tags)) {
          tags.forEach(tag => selectedTags.push(tag));
        } else {
          selectedTags.push(tags);
        }
      }
      return selectedTags;
    };

    const _tags = [];
    vm.tags = dialogTags.initTags(
      dialogTags.model.items.tags,
      _tags,
      selectedTags()
    );

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
      userInfo: news.data.userInfo,
      lastUpdateUserInfo: news.data.lastUpdateUserInfo,
      createDate: news.data.createDate,
      updateDate: news.data.updateDate,
      hasPrevious: () => {
        return news.data.hasPrevious;
      },
      urlPrevious: news.data.urlPrevious,
      hasNext: () => {
        return news.data.hasNext;
      },
      urlNext: news.data.urlNext,
      hasPreviousOrNext: () => {
        return news.data.hasPrevious || news.data.hasNext;
      },
      getDisplayMode: () => {
        return news.data.displayMode;
      },
      getNumberItemPerPage: () => {
        return news.data.numberItemPerPage;
      },
    };

    vm.filter = ()=> {
      const tags = [];
      vm.tags.forEach(function(tag) {
        if (tag.ticked) {
          tags.push(tag.id);
        }
      }, this);
      history.search('index', null);
      history.search('tags', tags);
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
