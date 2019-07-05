import history from '../../history';
import window from '../../window';
import q from '../../q';

const isApiUrl = function(response) {
  return response.config.url.indexOf(window.params.baseUrlJs) != -1;
};

export const responseError = function(response) {
  const url = history.url();
  // Afin d'évité une boucle récursive
  if (response.status === 401) {
    if (isApiUrl(response)) {
      // Redirect to connection
      window.location = '/utilisateur/connexion?url=' + url;
      return q.reject(response);
    }
  } else if (response.status === 403) {
    if (isApiUrl(response)) {
      // Redirect to connection
      window.location = '/utilisateur/non-authorise?url=' + url;
      return q.reject(response);
    }
  }

  return response;
};
