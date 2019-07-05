import app from '../app.module';
import { page } from '../shared/services/page-factory';

import { free } from './free-factory';
import view from './free.html';

const name = 'free';
class Controller {
  $onInit() {
    const _data = free.data;
    const title = free.getTitle(_data.elements);
    page.setTitle(title);

    const parentsJson = free.mapParent({
      type: 'div',
      childs: _data.elements,
    });
    const data = this;
    data.element = parentsJson;
    const metaParentsJson = free.mapParent({
      type: 'div',
      childs: _data.metaElements,
    });
    data.metaElement = metaParentsJson;
    data.data = {
      userInfo: _data.userInfo,
      lastUpdateUserInfo: _data.lastUpdateUserInfo,
      isDisplayAuthor: _data.data.isDisplayAuthor,
      isDisplaySocial: _data.data.isDisplaySocial,
      createDate: _data.createDate,
      updateDate: _data.updateDate,
    };

    return data;
    // const dispatch = redux.getDispatch();
    // dispatch(freeInit(data));

    //const connect = redux.getConnect();
    //this.unsubscribe = connect(this.mapStateToThis, {})(this);
  }
  /* $onDestroy() {
        this.unsubscribe();
    }
    mapStateToThis(state) {
        return state.free;
    }*/
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
