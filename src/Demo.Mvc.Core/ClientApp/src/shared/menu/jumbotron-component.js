import app from '../../app.module';
import redux from '../../redux';
import view from './jumbotron.html'

const name = 'jumbotron';

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
    ctrl.active = 0;
    return ctrl;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { master: state.master.masterData };
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
