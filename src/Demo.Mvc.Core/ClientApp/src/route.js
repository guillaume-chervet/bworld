import app from './app.module';
let _$route = null;

app.factory('dummyroute', [
  '$route',
  function($route) {
    _$route = $route;
    return {};
  },
]);

export default {
  current: () => _$route.current,
};
