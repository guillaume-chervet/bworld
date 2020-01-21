import app from '../app.module';
import { user } from '../user/info/user-factory';
import { site } from './master/site-factory';
import { stats } from './stats/stats-factory';
import { administration } from './hub/administration-factory';

app.config([
  '$routeProvider',
  function($routeProvider) {
    var initUser = [
      function() {
        return user.initAsync();
      },
    ];

    $routeProvider
      .when('/administration/site', {
        template: '<site-admin></site-admin>',
        resolve: {
          initUser: initUser,
          initFree: [
            function() {
              return site.initAsync();
            },
          ],
        },
      })
      .when('/administration/menu', {
        template: '<menu-admin></menu-admin>',
        resolve: {
          initUser: initUser,
        },
      })
      .when('/administration/statistiques', {
        template: '<stats></stats',
        resolve: {
          initUser: initUser,
          init: [
            function() {
              return stats.initAsync();
            },
          ],
        },
      })
      .when('/administration', {
        template: '<administration></administration>',
        resolve: {
          initUser: initUser,
          initAdministration: [
            function() {
              return administration.initAsync();
            },
          ],
        },
      });
  },
]);
