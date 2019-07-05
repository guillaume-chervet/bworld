import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { newsItem } from './newsItem-factory';
import { master } from '../../shared/providers/master-provider';
import { dialogTags } from '../../admin/tags/dialogTags-factory';
import view from './newsItem_admin.html';

const name = 'newsItemAdmin';

class Controller {
  $onInit() {
    const vm = this;
    const title = newsItem.getTitle(newsItem.data.elements);
    page.setTitle(title);

    const moduleId = master.getModuleId();

    const parentsJson = newsItem.mapParent({
      type: 'div',
      childs: newsItem.data.elements,
    });
    vm.element = parentsJson;

    const metaParentsJson = newsItem.mapParent({
      type: 'div',
      childs: newsItem.data.metaElements,
    });
    vm.metaElement = metaParentsJson;
    vm.data = newsItem.data.data;
    vm.submit = function() {
      if (newsItem.isUploading(vm.element.childs)) {
        return;
      }
      const tags = newsItem.data.data.tags;
      tags.length = 0;
      vm.inputFilters.forEach(function(tag) {
        if (tag.ticked) {
          tags.push(tag.id);
        }
      }, this);

      newsItem.saveAsync(moduleId);
    };

    vm.delete = function() {
      if (newsItem.isUploading(vm.element.childs)) {
        return;
      }
      module.deleteAsync(moduleId, newsItem.data.urlNews);
    };

    vm.isButtonDisabled = function() {
      return newsItem.isUploading(vm.element.childs);
    };

    const _tags = [];
    vm.inputFilters = dialogTags.initTags(
      dialogTags.model.items.tags,
      _tags,
      newsItem.data.data.tags
    );

    vm.openTags = function() {
      dialogTags.openAsync('items').then(function() {
        dialogTags.initTags(
          dialogTags.model.items.tags,
          _tags,
          newsItem.data.data.tags
        );
      });
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
