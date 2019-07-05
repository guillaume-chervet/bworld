import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import { newsItem } from './newsItem-factory';
import view from './newsItem.html';

var name = 'newsItem';

class Controller {
  $onInit() {
    const vm = this;
    const _data = newsItem.data;
    const title = newsItem.getTitle(_data.elements);

    page.setTitle(title);

    const parentsJson = newsItem.mapParent({
      type: 'div',
      childs: _data.elements,
    });
    vm.element = parentsJson;
    const metaParentsJson = newsItem.mapParent({
      type: 'div',
      childs: _data.metaElements,
    });
    vm.metaElement = metaParentsJson;
    vm.url = history.url();
    vm.data = {
      userInfo: _data.userInfo,
      lastUpdateUserInfo: _data.lastUpdateUserInfo,
      isDisplayAuthor: _data.data.isDisplayAuthor,
      isDisplaySocial: _data.data.isDisplaySocial,
      createDate: _data.createDate,
      updateDate: _data.updateDate,
      urlNews: _data.urlNews,
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
