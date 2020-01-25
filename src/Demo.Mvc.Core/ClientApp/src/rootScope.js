//const injector = window.angular.injector(['ng']);
//const $route = injector.get('$route');
//const $location = injector.get('$location');
import app from './app.module';
let _$rootScope = null;
const list = [];

app.factory('dummyrootscope', [
  '$rootScope',
  function($rootScope) {
    _$rootScope = $rootScope;

    list.forEach(e => _$rootScope.$on(e.event, e.func));
    list.length = 0;

    return {};
  },
]);

export default {
  $on: (event, func) => {
    if (_$rootScope) {
      _$rootScope.$on(event, func);
    } else {
      list.push({ event, func });
    }
  },
};
