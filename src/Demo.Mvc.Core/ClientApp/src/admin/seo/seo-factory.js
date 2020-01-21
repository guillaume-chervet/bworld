import { master } from '../../shared/providers/master-provider';
import { toast as toastr } from '../../shared/services/toastr-factory';
import $http from '../../http';

const data = {};

const initAsync = function() {
  const siteId = master.site.siteId;
  return $http
    .get(master.getUrl('api/seo/get/' + siteId))
    .then(function(response) {
      if (response && response.data) {
        const result = response.data.data;

        //result.Metas
        let disallow = '';
        if (result.disallows) {
          for (let i = 0; i < result.disallows.length; i++) {
            disallow += result.disallows[i] + '\n';
          }
        }
        data.disallow = disallow;
        data.redirects = result.redirects;
        let googleCode = '';
        let bingCode = '';
        if (result.metas) {
          for (let j = 0; j < result.metas.length; j++) {
            const meta = result.metas[j];
            if (meta.engine === 0) {
              googleCode = meta.code;
            }
            if (meta.engine === 1) {
              bingCode = meta.code;
            }
          }
        }
        data.googleCode = googleCode;
        data.bingCode = bingCode;
      }
    });
};

function saveAsync(model) {
  const disallows = [];
  const temps = model.disallow.split('\n');
  for (let i = 0; i < temps.length; i++) {
    const disa = temps[i];
    if (disa) {
      disallows.push(disa);
    }
  }

  const dataToSend = {
    siteId: master.site.siteId,
    seo: {
      metas: [
        {
          engine: 0,
          code: model.googleCode,
        },
        {
          engine: 1,
          code: model.bingCode,
        },
      ],
      disallows: disallows,
      redirects: model.redirects,
    },
  };

  return $http
    .post(master.getUrl('api/seo/save'), dataToSend)
    .then(function(response) {
      if (response.data.isSuccess) {
        toastr.success(
          'Sauvegarde effectuée avec succès.',
          'Sauvegarde moteur de recherche'
        );
      }
    });
}

export const seo = {
  initAsync: initAsync,
  saveAsync: saveAsync,
  data: data,
};
