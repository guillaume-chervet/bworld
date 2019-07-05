import app from './app.module';
let _$location = null;

app.factory('dummyhistory', [
  '$location',
  function($location) {
    _$location = $location;
    return {};
  },
]);

const search = function(queryString, value) {
  if (!_$location) {
    console.warn('_$location est null il y a un problème');
    return null;
  }
  if (queryString) {
    return _$location.search(queryString, value);
  }
  return _$location.search();
};

const path = function(path) {
  return _$location.path(path);
};

const protocol = function() {
  return _$location.protocol();
};

const host = function() {
  return _$location.host();
};

const port = function() {
  return _$location.port();
};

const url = function() {
  return _$location.url();
};

const absUrl = function() {
  return _$location.absUrl();
};

export default {
  search,
  path,
  protocol,
  host,
  port,
  url,
  absUrl,
};
