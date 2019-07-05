import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { dialog } from '../../shared/dialog/dialog-factory';
import { seo } from './seo-factory';

import { master } from '../../shared/providers/master-provider';
import viewGoogle from './dialogGoogle.html';
import viewRobot from './dialogRobot.html';
import view from './seoAdmin.html';

const name = 'seoAdmin';

class Controller {
  $onInit() {
    const ctrl = this;
    page.setTitle('Moteurs de recherche', page.types.admin);
    const link = {};
    const updateLink = function(data) {
      if (data.googleCode) {
        link.google = '/google' + data.googleCode + '.html';
      }

      if (data.bingCode) {
        link.bing = '/BingSiteAuth.xml';
      }
    };
    updateLink(seo.data);
    ctrl.rules = {
      googleCode: [],
      bingCode: [],
      disallow: [],
    };

    ctrl.seo = seo.data;
      ctrl.link = link;
      ctrl.getInternalPath = master.getInternalPath

    ctrl.dialogGoogle = function() {
      dialog.openAsync({
        template: viewGoogle,
        title: 'Aide google webmaster tool',
      });
    };

    ctrl.dialogBing = function() {
      dialog.openAsync({
        template: '<dialog-bing></dialog-bing>',
        title: 'Aide bing webmaster tool',
      });
    };

    ctrl.dialogRobot = function() {
      dialog.openAsync({
        template: viewRobot,
        title: 'Aide robots.txt',
      });
    };

    ctrl.submit = function(formSeo) {
      if (formSeo.$valid) {
        seo.saveAsync(ctrl.seo).then(function() {
          updateLink(ctrl.seo);
        });
      }
    };

    ctrl.add = function() {
      if (!ctrl.seo.redirects) {
        ctrl.seo.redirects = [];
      }

      ctrl.seo.redirects.push({
        pathSource: '',
        pathDestination: '',
      });
    };

    ctrl.delete = function(element) {
      const childs = ctrl.seo.redirects;
      while (childs.indexOf(element) !== -1) {
        childs.splice(childs.indexOf(element), 1);
      }
    };

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
