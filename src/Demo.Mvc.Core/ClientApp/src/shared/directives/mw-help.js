import app from '../../app.module';
import { device } from '../services/device-factory';
const name = 'mwHelp';

app.directive(name, [
  function() {
    return {
      restrict: 'EA',
      replace: false,
      compile: function(element, attrs) {
        if (!device.isMobileAndTabletcheck() && attrs.mwHelp) {
          var split = attrs.mwHelp.split(';');

          for (var i = 0; i < split.length; i++) {
            var elm = element.children().eq(i);
            var value = split[i];
            if (elm && value) {
              elm.attr('uib-popover', value);
              elm.attr('popover-placement', 'top-left');
              elm.attr('popover-trigger', '"mouseenter"');
            }
          }

          return function() {};
        }
      },
    };
  },
]);

export default name;
