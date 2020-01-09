import history from '../../history';
import window from '../../window';
import { master } from '../../shared/providers/master-provider';

const getPostUrl = () => {
  let postUrl = 'Account/ExternalLogin';
  if (window.params.master.domainLoginUrl) {
    postUrl = `${window.params.master.domainLoginUrl}/${postUrl}`;
  }
  return postUrl;
};

const getPostAssociationUrl = () => {
  let postUrl = 'Account/ExternalLoginAssociation';
  if (window.params.master.domainLoginUrl) {
    postUrl = `${window.params.master.domainLoginUrl}/${postUrl}`;
  }
  return postUrl;
};

const getFullBaseUrl = () => {
  return window.params.header.baseUrl;
};

const getReturnUrl = () => {
    let url = master.concatUrl(getFullBaseUrl(), window.params.header.baseUrlSite);
  const search = history.search();
  const path = search.url;
  if (path) {
    url = master.concatUrl(url, path);
  } else if (history.path().indexOf('connexion') === -1) {
    url = url + history.url();
  }
  return url;
};

const passwordRegex = new RegExp(
  '^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$',
  'i'
);
const rules = {
  password: [
    {
      pattern: {
        regex: passwordRegex,
        message:
          'Minimum 8 caractères 1 nombre 1 majuscule et 1 caractère spécial',
      },
    },
    'required',
    {
      maxLength: 100,
    },
    {
      minLength: 6,
    },
  ],
};

const formatReturnUrl = (returnUrl) => {
  if (!returnUrl) {
    return returnUrl;
  }
  return returnUrl.replace('://', '----').replace(':', '---');
};

export const login = {
  getPostUrl: getPostUrl,
  getPostAssociationUrl: getPostAssociationUrl,
  getFullBaseUrl: getFullBaseUrl,
  getReturnUrl: getReturnUrl,
  domainLoginUrl: window.params.master.domainLoginUrl,
  rules: rules,
  formatReturnUrl: formatReturnUrl,
};
