import app from '../../app.module';
import './elementCodePanel-component';
import './code.css';
import {react2angular} from "react2angular";
import React, {Fragment} from "react";
import {Code} from "./elementCode-component";
import {ElementAdmin} from "../elementAdmin-component";
import LoadableCodeAce from "./LoadableCodeAce";

export const ElementCodeAdmin = ({ element, onChange }) => {

  const onChangeWrapper = (newData) => {
    onChange({ what: "element-edit", element: {...element, data: {...element.data, ...newData } } });
  };
  
  return (
      <ElementAdmin
          onChange={onChangeWrapper}
          element={element}
          adminTitle={'Code'}
          adminEdit={<div className="mw-code-panel">
            {element.data.files.map(file => (
                <Fragment key={file.fileName}>
                  <h3>{file.fileName}</h3>
                  <LoadableCodeAce file={file} onChange={onChangeWrapper} />
                </Fragment>
            ))}</div>}
          adminView={<Code element={element} />}>
      </ElementAdmin>
  );
};

const name = 'elementCodeAdmin';

app.component(name, react2angular(ElementCodeAdmin, ['element', 'mode', 'onChange']));

export default name;