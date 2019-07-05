import app from '../../app.module';
import { notifications } from './hub/notifications-factory';
import { notificationItem } from './items/notificationItem-factory';
import { user } from '../../user/info/user-factory';
import { dialogTags } from '../tags/dialogTags-factory';
import { adminUser } from '../users/adminUser-factory';

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
      .when('/administration/notifications', {
        template: '<notifications></notifications>',
        resolve: {
          initDialogUserTags: initDialogUserTags,
          initUser: initUser,
          initFree: [
            function() {
              return notifications.initAsync();
            },
          ],
        },
      })
      .when('/administration/notifications/item/:moduleId/:title', {
        template: '<notification-item></notification-item>',
        resolve: {
          initDialogUserTags: initDialogUserTags,
          initAdministration: [
            function() {
              return adminUser.initAsync();
            },
          ],
          initUser: initUser,
          initFree: [
            function() {
              return notificationItem.initAsync();
            },
          ],
        },
      });
  },
]);
