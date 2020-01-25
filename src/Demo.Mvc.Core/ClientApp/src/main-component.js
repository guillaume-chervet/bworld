import app from './app.module';
import redux from './redux';
import history from './history';
import view from './main.html';
import './htmlheader.component';
const name = 'main';

class Controller {
  constructor($scope) {
    this.$scope = $scope;
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    var ctrl = this;
    this.$scope.$on('$routeChangeSuccess', function($event, next, current) {
      ctrl.currentPath = history.path();
    });
    return ctrl;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return {
      isDisplayMenu: state.master.menuData.isDisplayDivMenu,
      styleTemplate: state.master.masterData.styleTemplate,
    };
  }
  mapThisToProps() {
    return {};
  }
}
app.component(name, {
  template: view,
  controller: ['$scope', Controller],
});

export default name;
