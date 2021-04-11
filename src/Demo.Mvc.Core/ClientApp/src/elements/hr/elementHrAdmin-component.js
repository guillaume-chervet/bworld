import app from '../../app.module';
import {ElementAdmin} from "../elementAdmin-component";
import {react2angular} from "react2angular";
import React from "react";
import {HR} from "./elementHr-component";

const name = 'elementHrAdmin';

const HRAdmin = ({element, onChange}) => {
  return (<div className="form-group">
    <label htmlFor={element.property} className="col-sm-3 col-md-3 col-xs-12 control-label">Type</label>
    <div className="col-sm-9 col-md-9 col-xs-12">
      <select name="field" id={element.property} className="form-control" value={element.data.type} onChange={onChange}>
        <option value="">Petit</option>
        <option value="1">Moyen</option>
        <option value="2">Grand</option>
      </select>
    </div>
  </div>)
}

export const ElementHRAdmin = ({ element, mode, onChange }) => {
  const onChangeWrapper = (e) => {
    onChange({ what: "element-edit", element: {...element, data: {type: e.target.value}}});
  };
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Séparateur'}
          adminEdit={<HRAdmin
              element={element}
              onChange={onChangeWrapper}
          />}
          adminView={<HR element={element} />}>
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementHRAdmin, ['element', 'mode', 'onChange']));

export default name;
