import history from '../../history';
import cookie from '../../cookie';
import { master } from '../providers/master-provider';
import $http from '../../http';

let promise = null;
let clientSessionId = '';
let cookieSessionId = '';

let _lastPageName = '';
let _lastPageParam = '';

function postDataAsync(pageName, pageParam, type) {
  if (!pageParam) {
    pageParam = 'Page';
  }
  if (pageName === _lastPageName && pageParam === _lastPageParam) {
    return;
  }
  _lastPageName = pageName;
  _lastPageParam = pageParam;
  if (cookie.get('sessionId')) {
    cookieSessionId = cookie.get('sessionId');
  }
  let referrer = '';
  if (document.referrer) {
    referrer = document.referrer;
  }
  const url = history.path();
  if (url && url.toLowerCase().indexOf('://localhost') > 0) {
    return null;
  }
  let typeId = 0;
  if (type) {
    typeId = type.id;
  }
  const dataToSend = {
    name: pageName,
    action: pageParam,
    type: typeId,
    siteId: master.site.siteId,
    clientSessionId,
    cookieSessionId,
    referrer: referrer,
    url: url,
  };
  return $http
    .post(master.getUrl('api/stat/save'), dataToSend, {
      headers: {
        disableLoader: true,
      },
    })
    .then(function(response) {
      if (response.data.isSuccess) {
        clientSessionId = response.data.data.clientSessionId;
        if (!cookie.get('sessionId')) {
          cookie.put('sessionId', response.data.data.cookieSessionId);
        }
      }
    });
}

function trace(name, action, type) {
  if (promise === null) {
    promise = postDataAsync(name, action, type);
  } else {
    promise.then(postDataAsync(name, action, type));
  }
  return promise;
}

function getInfo() {
  const cookieSessionId = cookie.get('sessionId');
  return { cookieSessionId };
}

export const audit = {
  trace,
  getInfo,
};
