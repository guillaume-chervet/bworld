import app from '../../app.module';
import history from '../../history';
import { menu } from '../menu/menu-factory';
import redux from '../../redux';
import { master } from '../providers/master-provider';
import view from './mwSocialShare.html';

const name = 'mwSocialShare';

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
    const search = history.search();
    let isShow =
      search.display_menu === undefined || search.display_menu !== 'false';
    if (menu.isPrivate()) {
      isShow = false;
    }
    vm.isShow = isShow;
    return vm;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    const _master = state.master.masterData;
    let url = history.absUrl();
    if (url) {
      url = url.replace('#_=_', '');
    }
    const info = {
      titleSite: _master.titleSite,
      titlePage: _master.titlePage,
      logoUrl: master.getUrl(_master.logoUrl),
      text: _master.titleSite + ' : ' + _master.titlePage + ' ' + url,
    };
    return { info };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
});

export default name;
