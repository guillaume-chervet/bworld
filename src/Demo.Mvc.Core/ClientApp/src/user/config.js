import app from '../app.module';
import master from '../shared/providers/master-provider';
import { user } from './info/user-factory';
import { manageUser } from './info/manageUser-service';

app.config([
  '$routeProvider',
  master + 'Provider',
  function($routeProvider) {
    var initUser = [
      function() {
        return user.initAsync();
      },
    ];

    $routeProvider
      .when('/utilisateur', {
        template: '<user-panel></user-panel>',
        resolve: {
          initManageUser: [
            function() {
              return user.initAsync().then(function() {
                return manageUser.initAsync(true);
              });
            },
          ],
        },
      })
      .when('/utilisateur/compte', {
        template: '<user></user>',
        resolve: {
          initManageUser: [
            function() {
              return user.initAsync().then(function() {
                return manageUser.initAsync(false);
              });
            },
          ],
        },
      })
      .when('/utilisateur/connexion', {
        template: '<login></login>',
      })
      .when('/utilisateur/creation', {
        template: '<page-create-account></page-create-account>',
      })
      .when('/utilisateur/confirmation-compte-externe', {
        template: '<external-login-confirmation></external-login-confirmation>',
        resolve: {
          initUser: initUser,
        },
      })
      .when('/utilisateur/erreur-compte-externe', {
        template: '<external-login-failure></external-login-failure>',
        resolve: {
          initUser: initUser,
        },
      })
      .when('/utilisateur/confirmation-email', {
        template: '<confirm-email></confirm-email>',
      })
      .when('/utilisateur/confirmation-email-error', {
        template: '<confirm-email-error></confirm-email-error>',
      })
      .when('/utilisateur/reset-password', {
        template: '<reset-password></reset-password>',
      })
      .when('/utilisateur/reinit-password', {
        template: '<reinit-password></reinit-password>',
      })
      .when('/utilisateur/confirmation-association-compte-externe', {
        template: '<confirm-association></confirm-association>',
      })
      .when('/utilisateur/erreur-association-compte-externe', {
        template: '<confirm-association-error></confirm-association-error>',
      })
      .when('/utilisateur/non-authorise', {
        template: '<not-authorised></not-authorised>',
      });
  },
]);
