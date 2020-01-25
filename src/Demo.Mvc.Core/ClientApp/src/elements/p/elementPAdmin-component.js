import app from '../../app.module';
import { service as elementService } from '../element-factory';
import './TextEditor';
import view from './p_admin.html';

const name = 'elementPAdmin';

class Controller {
  $onInit() {
    const ctrl = this;
    elementService.inherit(ctrl);

    function clear_attr(str, attrs) {
      const reg2 = /\s*(\w+)=\"[^\"]+\"/gm;
      const reg = /<\s*(\w+).*?>/gm;
      str = str.replace(reg, function(match) {
        const r_ = match.replace(reg2, function(match_) {
          const reg2_ = /\s*(\w+)=\"[^\"]+\"/gm;
          const m = reg2_.exec(match_);
          if (m != null) {
            const attrName = m[1];
            if (attrs.indexOf(attrName) >= 0 || attrName === 'href') {
              return match_;
            }
          }
          return '';
        });
        return r_;
      });
      return str;
    }

    ctrl.element.data = clear_attr(ctrl.element.data, []);
    ctrl.onChange = function(html) {
      ctrl.element.data = html;
    };

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    mode: '<',
    onChange: '<',
  },
});
