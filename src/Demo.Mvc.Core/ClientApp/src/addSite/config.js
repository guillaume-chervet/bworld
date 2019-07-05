import app from '../app.module';
import { addSite } from './addSite-factory';
import { user } from '../user/info/user-factory';

app.config([
  '$routeProvider',
  function($routeProvider) {
    const initUser = [
      function() {
        return user.initAsync();
      },
    ];

    const initAddSite = [
      function() {
        return addSite.initAsync();
      },
    ];

    $routeProvider
      .when('/site/creation/confirmation', {
        template: '<add-site-confirm></add-site-confirm>',
        resolve: {
          iniUser: initUser,
          initAddSite: initAddSite,
        },
      })
      .when('/site/:moduleId/:title/configuration', {
        template: '<add-site-configuration></add-site-configuration>',
        resolve: {
          iniUser: initUser,
          initAddSite: initAddSite,
        },
      })
      .when('/site/:moduleId/:title/authentification', {
        template: '<add-site-authentification></add-site-authentification>',
        resolve: {
          iniUser: initUser,
          initAddSite: initAddSite,
        },
      })
      .when('/site/:moduleId/:title/validation', {
        template: '<add-site-submit></add-site-submit>',
        resolve: {
          iniUser: initUser,
          initAddSite: initAddSite,
        },
      })
      .when('/site/:moduleId/:title', {
        template: '<add-site></add-site>',
        resolve: {
          iniUser: initUser,
          initAddSite: initAddSite,
        },
      })
      .when('/administration/site/:moduleId/:title', {
        template: '<add-site-admin></add-site-admin>',
        resolve: {
          iniUser: initUser,
          initAddSite: initAddSite,
        },
      });
  },
]);
