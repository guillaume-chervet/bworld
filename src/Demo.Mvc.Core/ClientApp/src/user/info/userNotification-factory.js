import { master } from '../../shared/providers/master-provider';
import $http from '../../http';

const data = {
  numberUnreadMessage: 0,
  numberSiteUnreadMessage: 0,
};
const _isUnreadMessage = function() {
  return data.numberUnreadMessage > 0;
};
const _isSiteUnreadMessage = function() {
  return data.numberSiteUnreadMessage > 0;
};
data.isUnreadMessage = _isUnreadMessage;
data.isSiteUnreadMessage = _isSiteUnreadMessage;

const initAsync = function() {
  const siteId = master.site.siteId;
  return $http
    .get(master.getUrl('api/user/notification/' + siteId), {
      headers: {
        disableLoader: true,
      },
    })
    .then(function(response) {
      if (response) {
        if (response.data.isSuccess) {
          var result = response.data.data;
          data.numberUnreadMessage = result.numberUnreadMessage;
          data.numberSiteUnreadMessage = result.numberSiteUnreadMessage;
        }
      }
    });
};

export const userNotification = {
  data: data,
  initAsync: initAsync,
};
