import cookie from '../../cookie';
import window from '../../window';

const isLocalStorage = window.localStorage && window.localStorage.setItem;

const _put = function(key, value) {
  const jsonValue = JSON.stringify(value);
  if (isLocalStorage) {
    window.localStorage.setItem(key, jsonValue);
  } else {
    cookie.put(key, jsonValue);
  }
};

const _get = function(key) {
  let jsonValue = null;
  if (isLocalStorage) {
    jsonValue = window.localStorage.getItem(key);
  } else {
    jsonValue = cookie.get(key);
  }
  if (jsonValue) {
    return JSON.parse(jsonValue);
  }
  return null;
};

const _remove = function(key) {
  if (isLocalStorage) {
    window.localStorage.removeItem(key);
  } else {
    cookie.remove(key);
  }
};

export const localStorage = {
  put: _put,
  get: _get,
  remove: _remove,
};
