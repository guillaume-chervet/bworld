import app from '../app.module';
import { page } from '../shared/services/page-factory';
import { master } from '../shared/providers/master-provider';
import { addSite } from './addSite-factory';
import view from './addSiteConfiguration.html';

import './addSiteConfiguration.css';

const name = 'addSiteConfiguration';

class Controller {
  $onInit() {
    const vm = this;
    page.setTitle('Création site configuration');
    vm.data = addSite.data;
    vm.site = addSite.site;
    vm.rules = {
      category: 'required',
      domain: 'required',
    };
    vm.getDomain = addSite.getDomain;
    vm.getDomainTemporary = addSite.getDomainTemporary;

    let badSiteAdress = null;
    vm.onChange = function(form) {
      if (badSiteAdress) {
        if (badSiteAdress != vm.site.domain) {
          // On efface le message d'erreur
          form.uDomain.mw.setValidity('SITE_ADRESS_ALREADY_EXIST', true);
        }
      }
    };

    vm.navBack = addSite.navBack;
    vm.submit = function(form) {
      if (form.$valid) {
        const dataToSend = {
          categoryId: vm.site.category,
          moduleId: master.getModuleId(),
          siteName: vm.site.domain,
          siteId: master.site.siteId,
        };
        form.uDomain.mw.setValidity('SITE_ADRESS_ALREADY_EXIST', true);
        addSite.checkAsync(dataToSend).then(function(result) {
          if (!result.isSuccess) {
            if (result.validationResult.errors.sitE_ADRESS_ALREADY_EXIST) {
              badSiteAdress = form.uDomain.$viewValue;
              form.uDomain.mw.setValidity(
                'SITE_ADRESS_ALREADY_EXIST',
                false,
                'Cette adresse ' + form.uDomain.$viewValue + ' existe déjà.'
              );
            }
          } else {
            addSite.navNext();
          }
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
