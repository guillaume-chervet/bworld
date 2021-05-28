import {ElementAdmin} from "../elementAdmin-component";
import React from "react";

import './file_admin.css';
import {GalleryFile} from "./elementFile-component";
import {Upload} from "./upload-component";

export const ElementFileAdmin = ({ element, mode, onChange }) => {
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Images'}
          adminEdit={<Upload element={element} onChange={onChange} mode={mode}/>}
          adminView={<GalleryFile element={element} />}>
      </ElementAdmin>
  );
};
