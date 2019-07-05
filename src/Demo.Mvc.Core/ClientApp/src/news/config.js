import app from '../app.module';
import { user } from '../user/info/user-factory';
import itemStates from '../shared/itemStates';
import { news } from './news/news-factory';
import { newsItem } from './newsItem/newsItem-factory';
import { dialogTags } from '../admin/tags/dialogTags-factory';

app.config([
  '$routeProvider',
  function($routeProvider) {
    const initUser = [
      function() {
        return user.initAsync();
      },
    ];

    const initNews = [
      function() {
        return user.initAsync().then(function() {
          const states = [itemStates.published];
          return news.initAsync(null, states);
        });
      },
    ];

    const initDialogUserTags = [
      function() {
        return dialogTags.initAsync('items');
      },
    ];

    $routeProvider
      .when('/administration/:private/articles/item/:moduleId/:title', {
        template: '<news-item-admin></news-item-admin>',
        resolve: {
          initDialogUserTags,
          initUser: initUser,
          initFree: [
            function() {
              return newsItem.initAsync();
            },
          ],
        },
      })
      .when('/administration/articles/item/:moduleId/:title', {
        template: '<news-item-admin></news-item-admin>',
        resolve: {
          initDialogUserTags,
          initUser: initUser,
          initFree: [
            function() {
              return newsItem.initAsync();
            },
          ],
        },
      })
      .when('/:private/articles/item/:moduleId/:title', {
        template: '<news-item></news-item>',
        resolve: {
          initDialogUserTags,
          initUser: initUser,
          initFree: [
            function() {
              return newsItem.initAsync();
            },
          ],
        },
      })
      .when('/articles/item/:moduleId/:title', {
        template: '<news-item></news-item>',
        resolve: {
          initDialogUserTags,
          initUser: initUser,
          initFree: [
            function() {
              return newsItem.initAsync();
            },
          ],
        },
      })
      .when('/administration/:private/articles/:moduleId/:title', {
        template: '<news-admin></news-admin>',
        resolve: {
          initDialogUserTags,
          initUser: initUser,
          initFree: [
            function() {
              return news.initAsync(null, null);
            },
          ],
        },
      })
      .when('/administration/articles/:moduleId/:title', {
        template: '<news-admin></news-admin>',
        resolve: {
          initDialogUserTags,
          initUser: initUser,
          initFree: [
            function() {
              return news.initAsync(null, null);
            },
          ],
        },
      })
      .when('/:private/articles/:moduleId/:title', {
        template: '<news></news>',
        resolve: {
          initDialogUserTags,
          initNews: initNews,
        },
      })
      .when('/articles/:moduleId/:title', {
        template: '<news></news>',
        resolve: {
          initDialogUserTags,
          initNews: initNews,
        },
      });
  },
]);
