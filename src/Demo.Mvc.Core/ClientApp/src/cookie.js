import app from './app.module';
let _$cookieStore = null;

app.factory('dummycookie', [
  '$cookies',
  function($cookieStore) {
    _$cookieStore = $cookieStore;
    return {};
  },
]);

export default {
  get: name => _$cookieStore.getObject(name),
  remove: name => _$cookieStore.remove(name),
  put: (name, value) => _$cookieStore.putObject(name, value),
};
