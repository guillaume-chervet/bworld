import app from '../../app.module';
const name = 'jsonld';

app.directive(name, [
  '$filter',
  '$sce',
  function($filter, $sce) {
    return {
      restrict: 'E',
      template: function() {
        return '<script type="application/ld+json" ng-bind-html="onGetJson()"></script>';
      },
      scope: {
        json: '=json',
      },
      link: function(scope) {
        scope.onGetJson = function() {
          return $sce.trustAsHtml($filter('json')(scope.json));
        };
      },
      replace: true,
    };
  },
]);

export default name;
