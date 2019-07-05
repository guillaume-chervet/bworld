import app from './app.module';
import { dialogTags } from './admin/tags/dialogTags-factory';
import { user } from './user/info/user-factory';
import modulesFactory from './modules-factory';
import redux from './redux';
import itemStates from './shared/itemStates';
import { getInitialState } from './shared/providers/master-reducer';

app.config([
  '$routeProvider',
  '$httpProvider',
  '$locationProvider',
  '$sceProvider',
  '$sceDelegateProvider',
  '$compileProvider',
  function(
    $routeProvider,
    $httpProvider,
    $locationProvider,
    $sceProvider,
    $sceDelegateProvider,
    $compileProvider
  ) {
    if (!window.params.isDebug) {
      $compileProvider.debugInfoEnabled(false);
    }

    $compileProvider.debugInfoEnabled(window.params.isDebug);
    $sceProvider.enabled(true);
    $sceDelegateProvider.resourceUrlWhitelist([
      // Allow same origin resource loads.
      'self',
      // Allow loading from our assets domain.  Notice the difference between * and **.
      window.params.mainDomainUrl + '/**',
    ]);
    $httpProvider.defaults.withCredentials = true;
    $httpProvider.defaults.headers.common['X-Requested-With'] =
      'XMLHttpRequest';
    const masterProvider = getInitialState();
    const homePageInfo = masterProvider.homePageInfo;
    const homePagePrivateInfo = masterProvider.homePagePrivateInfo;
    $locationProvider.html5Mode(true).hashPrefix('!');

    $routeProvider
      .when('/', {
        template:
          '<' + homePageInfo.viewName + '></' + homePageInfo.viewName + '>',
        resolve: {
          init: [
            '$q',
            function($q) {
              const service = modulesFactory.getModule(homePageInfo.serviceName)
                .service;
              if (service && service.initAsync) {
                const promiseTags = dialogTags.initAsync('items');
                const promiseModule = user.initAsync().then(function() {
                  const state = redux.getState();
                  const _user = state.user;
                  const states = _user.user.isAdministrator
                    ? [itemStates.draft, itemStates.published]
                    : [itemStates.published];
                  return service.initAsync(homePageInfo.menuKey, states);
                });
                return $q.all([promiseModule, promiseTags]);
              }
              return null;
            },
          ],
        },
      })
      .when('/privee', {
        template:
          '<' +
          homePagePrivateInfo.viewName +
          '></' +
          homePagePrivateInfo.viewName +
          '>',
        resolve: {
          init: [
            '$q',
            function($q) {
              const service = modulesFactory.getModule(homePageInfo.serviceName)
                .service;
              if (service && service.initAsync) {
                const promiseTags = dialogTags.initAsync('items');
                const promiseModule = user.initAsync().then(function() {
                  const state = redux.getState();
                  const _user = state.user;
                  const states = _user.user.isAdministrator
                    ? [itemStates.draft, itemStates.published]
                    : [itemStates.published];
                  return service.initAsync(homePageInfo.menuKey, states);
                });
                return $q.all([promiseModule, promiseTags]);
              }
              return null;
            },
          ],
        },
      })
      .when('/401', {
        template: '<401></401>',
      })
      .otherwise({
        redirectTo: '/',
      });
  },
]);
