//const injector = window.angular.injector(['ng']);
//const $route = injector.get('$route');
//const $location = injector.get('$location');
import app from './app.module';
let _$modal = null;

app.factory('dummymodal', [
  '$uibModal',
  function($modal) {
    _$modal = $modal;
    return {};
  },
]);

export default {
  open: args => _$modal.open(args),
};
