import app from '../app.module';
import view from './pageBreadcrumb.html';
import redux from "../redux";

const name = 'pageBreadcrumb';

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
        this.mapStateToThis,
        this.mapThisToProps
    )(this);
  }
  $onInit() {
    const ctrl = this;
    return ctrl;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { master: state.master };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: Controller,
  transclude: {
    content: 'content',
  },
});

export default name;
