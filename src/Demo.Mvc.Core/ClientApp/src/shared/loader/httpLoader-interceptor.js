import { loader } from './loader-service';

export const request = function(config) {
  if (!config) {
    config = {
      headers: {
        method: 'GET',
      },
    };
  }
  var method = config.method;
  if (config.headers.method) {
    method = config.headers.method;
    delete config.headers.method;
  }
  if (!config.headers.disableLoader) {
    if (config.headers.loaderMessage) {
      loader.add(config.headers.loaderMessage);
      delete config.headers.loaderMessage;
    } else {
      if (method === 'POST' || method === 'PUT') {
        loader.add('Enregistrement...');
      } else if (method == 'DELETE') {
        loader.add('Supression...');
      } else {
        loader.add();
      }
    }
  }

  if (config.headers.disableLoader !== undefined) {
    delete config.headers.disableLoader;
  }
  // do something on success
  return config;
  //return config || $q.when(config);
};
