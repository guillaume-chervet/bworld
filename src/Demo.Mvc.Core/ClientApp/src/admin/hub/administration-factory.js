import { master } from '../../shared/providers/master-provider';
import $http from '../../http';

const _administration = {};

const initAsync = function() {
  const siteId = master.site.siteId;
  return $http
    .get(master.getUrl('api/admin/get/' + siteId))
    .then(function(response) {
      if (response) {
        if (response.data.isSuccess) {
          /*   var data = response.data.data;

                    administration.totalSizeBytes = bytesToSize(data.totalSizeBytes);
                  administration.maxTotalSizeBytes = bytesToSize(data.maxTotalSizeBytes);
                  administration.percentageTotalSizeBytes = Math.floor((data.totalSizeBytes / data.maxTotalSizeBytes) * 100);
              */
        }
      }
    });
};

/* function bytesToSize(bytes) {
     var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
     if (bytes === 0) return 'n/a';
     var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
     return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[[i]];
 }*/

export const administration = {
  initAsync: initAsync,
  administration: _administration,
};
