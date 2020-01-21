import app from '../../app.module';
const name = 'text';

app.directive(name, [
  '$sanitize',
  function($sanitize) {
    var linker = function(scope, element) {
      function replace(text, oldString, newString) {
        return text.split(oldString).join(newString);
      }

      element.html(
        replace(replace($sanitize(scope.text), '\n', '<br/>'), '&#10;', '<br/>')
      );

      scope.$watch('text', function() {
        element.html(
          replace(
            replace($sanitize(scope.text), '\n', '<br/>'),
            '&#10;',
            '<br/>'
          )
        );
      });
    };

    return {
      restrict: 'EA',
      scope: {
        text: '=',
      },
      replace: true,
      link: linker,
    };
  },
]);

export default name;
