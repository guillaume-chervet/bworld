import app from '../../app.module';
import route from '../../route';
import { user } from '../../user/info/user-factory';
import { dialogTags } from '../tags/dialogTags-factory';
import { adminUser } from './adminUser-factory';

app.config([
  '$routeProvider',
  function($routeProvider) {
    const initUser = [
      function() {
        return user.initAsync();
      },
    ];

    const initDialogUserTags = [
      function() {
        return dialogTags.initAsync();
      },
    ];

    $routeProvider
      .when('/administration/utilisateurs', {
        template: '<admin-user></admin-user>',
        resolve: {
          initDialogUserTags: initDialogUserTags,
          initUser: initUser,
          initAdministration: [
            function() {
              return adminUser.initAsync();
            },
          ],
        },
      })
      .when('/administration/utilisateurs/edition', {
        template: '<admin-edit-user></admin-edit-user>',
        resolve: {
          initDialogUserTags: initDialogUserTags,
          initUser: initUser,
          initAdministration: [
            function() {
              return adminUser.initUserAsync(null);
            },
          ],
        },
      })
      .when('/administration/utilisateurs/edition/:id', {
        template: '<admin-edit-user></admin-edit-user>',
        resolve: {
          initDialogUserTags: initDialogUserTags,
          initUser: initUser,
          initAdministration: [
            function() {
              return adminUser.initUserAsync(route.current().params.id);
            },
          ],
        },
      });
  },
]);
