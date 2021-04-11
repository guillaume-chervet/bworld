import app from '../../app.module';
import {react2angular} from "react2angular";
import React from "react";

const name = 'elementCheckboxAdmin';

const CheckboxAdmin = ({element, onChange}) => {
  return (<div className="form-group">
          <label htmlFor={element.property} className="col-sm-3 col-xs-12 control-label">{element.label}</label>
          <div className="checkbox col-sm-9 col-xs-12">
              <label>
                  <input id={element.property} type="checkbox" name={element.property}
                         value={element.data} />
                  {element.placeholder}
              </label>
          </div>
      </div>

  )
}

export const ElementCheckboxAdmin = ({ element, mode, onChange }) => {
  const onChangeWrapper = (e) => {
    onChange({ what: "element-edit", element: {...element, data:e.target.value}});
  };
  return (
      <CheckboxAdmin
          element={element}
          onChange={onChangeWrapper}
      />
  );
};

app.component(name, react2angular(ElementCheckboxAdmin, ['element', 'mode', 'onChange']));

export default name;