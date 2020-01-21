import app from '../app.module';
import { free } from './free-factory';
import { user } from '../user/info/user-factory';

app.config([
  '$routeProvider',
  function($routeProvider) {
    var initUser = [
      function() {
        return user.initAsync();
      },
    ];

    $routeProvider
      .when('/administration/page/:moduleId/:title', {
        template: '<free-admin></free-admin>',
        resolve: {
          initUser: initUser,
          initFree: [
            function() {
              return free.initAsync();
            },
          ],
        },
      })
      .when('/administration/:private/page/:moduleId/:title', {
        template: '<free-admin></free-admin>',
        resolve: {
          initUser: initUser,
          initFree: [
            function() {
              return free.initAsync();
            },
          ],
        },
      })
      .when('/page/:moduleId/:title', {
        template: '<free></free>',
        resolve: {
          initFree: [
            function() {
              return user.initAsync().then(() => free.initAsync());
            },
          ],
        },
      })
      .when('/:private/page/:moduleId/:title', {
        template: '<free></free>',
        resolve: {
          iniUser: initUser,
          initFree: [
            function() {
              return free.initAsync();
            },
          ],
        },
      });
  },
]);
