import app from '../app.module';
import { user } from '../user/info/user-factory';
import { sites } from './sites/sites-factory';
import { users } from './users/users-factory';

app.config([
  '$routeProvider',
  function($routeProvider) {
    var initUser = [
      function() {
        return user.initAsync();
      },
    ];

    $routeProvider
      .when('/super-administration/utilisateurs', {
        template: '<users-admin></users-admin>',
        resolve: {
          initUser: initUser,
          init: [
            function() {
              return users.initAsync();
            },
          ],
        },
      })
      .when('/super-administration/sites', {
        template: '<sites-admin></sites-admin>',
        resolve: {
          initUser: initUser,
          init: [
            function() {
              return sites.initAsync();
            },
          ],
        },
      })
      .when('/super-administration/modules', {
        template: '<modules-admin></modules-admin>',
        resolve: {
          initUser: initUser,
        },
      })
      .when('/super-administration/logs', {
        template: '<logs></logs>',
        resolve: {
          initUser: initUser,
        },
      })
      .when('/super-administration', {
        template: '<super-administration></super-administration>',
        resolve: {
          initUser: initUser,
        },
      });
  },
]);
