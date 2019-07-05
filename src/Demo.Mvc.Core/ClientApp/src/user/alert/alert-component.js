import app from '../../app.module';
import redux from '../../redux';
import view from './alert.html';

const name = 'mwAlert';

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

    ctrl.addAlert = function() {
      ctrl.alerts.push({
        msg: 'Another alert!',
      });
    };

    ctrl.closeAlert = function(index) {
      ctrl.alerts.splice(index, 1);
    };

    ctrl.isDisplayAlert = function() {
      if (ctrl.alerts && ctrl.alerts.length <= 0) {
        return false;
      }
      const state = redux.getState();
      return state.master.menuData.isDisplayMenu;
    };

    return this;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { alerts: state.user.alerts };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    text: '<content',
  },
});

export default name;
