import app from '../app.module';
import { user } from '../user/info/user-factory';
import { social } from './social-factory';

app.config([
  '$routeProvider',
  function($routeProvider) {
    var initUser = [
      function() {
        return user.initAsync();
      },
    ];

    $routeProvider
      .when('/administration/:private/social/:moduleId/:title', {
        template: '<social-admin></social-admin>',
        resolve: {
          initUser: initUser,
          initFree: [
            function() {
              return social.initAsync();
            },
          ],
        },
      })
      .when('/administration/social/:moduleId/:title', {
        template: '<social-admin></social-admin>',
        resolve: {
          initUser: initUser,
          initFree: [
            function() {
              return social.initAsync();
            },
          ],
        },
      });
  },
]);
