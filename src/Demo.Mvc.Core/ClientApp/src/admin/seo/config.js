import app from '../../app.module';
import { seo } from './seo-factory';
import { user } from '../../user/info/user-factory';

app.config([
  '$routeProvider',
  function($routeProvider) {
    const initUser = [
      function() {
        return user.initAsync();
      },
    ];

    $routeProvider.when('/administration/seo', {
      template: '<seo-admin></seo-admin>',
      resolve: {
        initUser: initUser,
        initSeo: [
          function() {
            return seo.initAsync();
          },
        ],
      },
    });
  },
]);
