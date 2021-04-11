import app from '../../app.module';
import {react2angular} from "react2angular";
import React from "react";

const name = 'elementColorAdmin';


export const ElementColorAdmin = ({ element, mode, onChange }) => {
  const onChangeWrapper = (e) => {
    onChange({ what: "element-edit", element: {...element, data: {url: e.target.value}}});
  };
  return (
      <div className="form-group form-group-lg">
        <label htmlFor={element.property} className="col-sm-3 col-xs-12 control-label">{element.label}</label>
        <div className="col-sm-2 col-xs-12">
            <input type="color" id="head" name={element.property}
                   value={element.data} onChange={onChangeWrapper} />
          
        </div>
      </div>
  );
};

app.component(name, react2angular(ElementColorAdmin, ['element', 'mode', 'onChange']));

export default name;
