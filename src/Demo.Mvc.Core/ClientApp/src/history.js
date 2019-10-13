import app from './app.module';
let _$location = null;

app.factory('dummyhistory', [
  '$location',
  function($location) {
    _$location = $location;
    return {};
  },
]);

const search = function(queryString, pathDestination) {
  if (!_$location) {
    console.warn('_$location est null il y a un problème');
    return null;
  }
  if (queryString) {
    let newPath = (pathDestination ? pathDestination  : path()) + "?";
    for (let name in queryString){
      const value = queryString[name];
      if(value !== null && value !== undefined) {
        if(value instanceof Array){
          if(value.length > 0){
           let values = "";
            value.forEach((v, index) => index > 0 ?  values += `&${v}` :  values +=  v );
            newPath += name + "=" + values;
          }
        } else {
          newPath += name + "=" + value;  
        }
        
        
      }
    } 
    path(newPath);
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
