import history from '../../../history';

const _externalLogin = {};

const init = function() {
  const searchObject = history.search();
  _externalLogin.email = searchObject.email;
  _externalLogin.provider = searchObject.provider;
  _externalLogin.returnUrl = searchObject.returnUrl;
};

const getReturnUrl = (returnUrl) => {
  if (returnUrl) {
    // HACK a cause d'un bug angularJS
    returnUrl = returnUrl.replace('----', '://').replace('---', ':');
  }
  return returnUrl;
};

export const externalLogin = {
  externalLogin: _externalLogin,
  init,
  getReturnUrl,
};
