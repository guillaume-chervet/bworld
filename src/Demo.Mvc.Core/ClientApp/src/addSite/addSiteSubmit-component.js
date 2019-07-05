import app from '../app.module';
import history from '../history';
import { page } from '../shared/services/page-factory';
import { master } from '../shared/providers/master-provider';
import { addSite } from './addSite-factory';
import view from './addSiteSubmit.html';

import './addSiteSubmit.css';

const name = 'addSiteSubmit';

class Controller {
  $onInit() {
    page.setTitle('Création site configuration');

    const vm = this;
    vm.data = addSite.data;
    vm.site = addSite.site;
    vm.rules = {
      condition: [
        'required',
        {
          equal: {
            equal: true,
            message:
              "Vous devez accèpter les condition générales d'utilisation.",
          },
        },
      ],
    };
    vm.getDomain = addSite.getDomain;
    vm.navBack = addSite.navBack;
    vm.submit = function(form) {
      if (form.$valid) {
        const dataToSend = {
          categoryId: vm.site.category,
          moduleId: master.getModuleId(),
          site: master.site,
          siteId: master.site.siteId,
          siteName: vm.site.domain,
        };

        let port = history.port();
        if (port === 80) {
          port = null;
        }

        dataToSend.port = port;
        dataToSend.isSecure = history.protocol() === 'https';

        vm.navBack = addSite.navBack;
        addSite.saveAsync(dataToSend).then(function() {
          /*if (!result.isSuccess) {
                        if (result.validationResult.errors.sitE_ADRESS_ALREADY_EXIST) {
    
    
                        }
                    }*/
        });
      }
    };

    return vm;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
