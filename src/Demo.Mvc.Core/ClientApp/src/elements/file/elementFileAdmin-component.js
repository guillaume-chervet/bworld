import app from '../../app.module';
import {ElementAdmin} from "../elementAdmin-component";
import React from "react";

import './file_admin.css';
import {GalleryFile} from "./elementFile-component";
import {Upload} from "./upload-component";
import {react2angular} from "react2angular";

const name = 'elementFileAdmin';

export const ElementFileAdmin = ({ element, mode, onChange }) => {
  const onChangeWrapper = (html) => {
    onChange({ what: "element-edit", element: {...element, data:html}});
  };
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Images'}
          adminEdit={<Upload element={element} onChange={onChangeWrapper} mode={mode}/>}
          adminView={<GalleryFile element={element} isAdmin={true} />}>
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementFileAdmin, ['element', 'mode', 'onChange']));

export default name;
