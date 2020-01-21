import app from '../../app.module';
const name = 'mwMeta';

app.directive(name, [
  function() {
    return {
      restrict: 'EA',
      replace: false,
      compile: function() {
        return function(scope, elm, attr) {
          if (attr.mwMeta) {
            var destroyWatch = scope.$watch(
              attr.mwHelp,
              function(newValue) {
                if (newValue) {
                  elm.attr('content', newValue);
                }
              },
              false
            );

            scope.$on('$destroy', function() {
              destroyWatch();
            });
          }
        };
      },
    };
  },
]);

export default name;
