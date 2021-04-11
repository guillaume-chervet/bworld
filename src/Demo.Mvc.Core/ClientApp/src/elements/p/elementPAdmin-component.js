import app from '../../app.module';
import { LoadableRteEditor } from './TextEditor';
import { Paragraphe } from './elementP-component';
import { ElementAdmin } from '../elementAdmin-component';
import React from 'react';
import { react2angular } from 'react2angular';

const name = 'elementPAdmin';

const clear_attr = (str, attrs) => {
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
};

export const ElementPAdmin = ({ element, mode, onChange }) => {
  const data = clear_attr(element.data, []);
  const onChangeWrapper = (html) => {
    onChange({ what: "element-edit", element: {...element, data:html}});
  };
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Texte'}
          adminEdit={<LoadableRteEditor
              value={data}
              onChange={onChangeWrapper}

          />}
          adminView={<Paragraphe element={element} />}>
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementPAdmin, ['element', 'mode', 'onChange']));

export default name;
