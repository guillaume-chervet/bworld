import app from '../../app.module';
import './loader.css';
import view from './loader.html';

const name = 'loader';
class Controller {
  constructor($ngRedux, $scope) {
    const vm = this;
    //vm.loader = loader;
    const unsubscribe = $ngRedux.connect(this.mapStateToThis, {})(this);
    $scope.$on('$destroy', unsubscribe);
    return vm;
  }
  mapStateToThis(state) {
    return state.loader;
  }
}

Controller.$inject = ['$ngRedux', '$scope'];

app.component(name, {
  template: view,
  controller: Controller,
  controllerAs: 'vm',
  bindings: {},
});

export default name;
