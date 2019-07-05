//const injector = window.angular.injector(['ng']);
//const $route = injector.get('$route');
//const $location = injector.get('$location');
import app from './app.module';
let _$sce = null;

app.factory('dummysce', [
  '$sce',
  function($sce) {
    _$sce = $sce;
    return {};
  },
]);

export default {
  trustAsResourceUrl: url => _$sce.trustAsResourceUrl(url),
};
