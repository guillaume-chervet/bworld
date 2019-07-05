import app from '../../app.module';
import { messages } from './messages-factory';
import { message } from './message-factory';
import { user } from '../../user/info/user-factory';

app.config([
  '$routeProvider',
  function($routeProvider) {
    $routeProvider
      .when('/utilisateur/messages', {
        template: '<messages></messages>',
        resolve: {
          init: [
            function() {
              return user.initAsync().then(function(userId) {
                return messages.initAsync(false, userId);
              });
            },
          ],
        },
      })
      .when('/utilisateur/messages/message/:chatId', {
        template: '<message></message>',
        resolve: {
          init: [
            function() {
              return user.initAsync().then(function(userId) {
                return message.initAsync(false, userId);
              });
            },
          ],
        },
      });
  },
]);
