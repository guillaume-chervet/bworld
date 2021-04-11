import app from '../../app.module';
import {react2angular} from "react2angular";
import React from "react";

const name = 'elementSelectAdmin';

const SelectAdmin = ({element, onChange}) => {
  return (<div className="form-group form-group-lg">
        <label htmlFor={element.property} className="col-sm-3 col-xs-12 control-label">Thème</label>
        <div className="checkbox col-sm-4 col-xs-12">
          <select id={element.property} name="uTheme" className="form-control" value={element.data} onChange={onChange}>
            <option value="">- Sélectionner -</option>
            <option value="default">Default</option>
            <option value="theme1">Theme 1</option>
          </select>
        </div>
      </div>
  )
}

export const ElementSelectAdmin = ({ element, mode, onChange }) => {
  const onChangeWrapper = (e) => {
    onChange({ what: "element-edit", element: {...element, data:e.target.value}});
  };
  return (
      <SelectAdmin
          element={element}
          onChange={onChangeWrapper}
      />
  );
};

app.component(name, react2angular(ElementSelectAdmin, ['element', 'mode', 'onChange']));

export default name;
