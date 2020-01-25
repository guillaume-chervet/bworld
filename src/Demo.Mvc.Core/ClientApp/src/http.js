import { request } from './shared/loader/httpLoader-interceptor';
import { responseError as responseErrorAuthentification } from './shared/interceptors/httpAuthentification-interceptor';
import { responseError } from './shared/interceptors/httpException-interceptor';
import { loader } from './shared/loader/loader-service';
import app from './app.module';
import q from './q';
import { convertStringDateToDateObject } from './shared/date/date-factory';

let _$http = null;

app.factory('dummyhttp', [
  '$http',
  function($http) {
    _$http = $http;
    return {};
  },
]);

const reponse = promise => {
  return promise.then(
    function(_response) {
      loader.remove();
      if (_response && _response.data) {
        const data = convertStringDateToDateObject(_response.data);
        return { ..._response, data };
      }
      return _response;
    },
    function(reason) {
      const qErrorAuthentification = responseErrorAuthentification(reason);
      const qError = responseError(reason);
      const clear = () => loader.clear();
      return q.all([qError, qErrorAuthentification]).then(clear, clear);
    }
  );
};

const post = function(url, data, config = null) {
  request(config);
  return reponse(_$http.post(url, data, config));
};

const get = function(url, config = null) {
  request(config);
  return reponse(_$http.get(url, config));
};

const deletefunc = function(url, config = null) {
  request(config);
  return reponse(_$http.delete(url, config));
};

const angular = {
  get,
  post,
  delete: deletefunc,
};

export default angular;
