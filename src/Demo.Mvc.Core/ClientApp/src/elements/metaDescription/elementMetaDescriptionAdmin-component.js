import app from '../../app.module';
import {react2angular} from "react2angular";
import React from "react";

const name = 'elementMetaDescriptionAdmin';


const MetaDescriptionAdmin = ({element, onChange}) => {
  return (<div className="form-group  form-group-lg">
        <label htmlFor={element.property} className="col-sm-3 col-xs-12 control-label">{element.label}</label>
        <div className="col-sm-4 col-xs-12">
          <textarea rows="4" name="field" id={element.property} value={element.data} onChange={onChange}
                    className="form-control"></textarea>
        </div>
      </div>
  )
}

export const ElementMetaDescriptionAdmin = ({ element, mode, onChange }) => {
  const onChangeWrapper = (e) => {
    onChange({ what: "element-edit", element: {...element, data:e.target.value}});
  };
  return (
      <MetaDescriptionAdmin
          element={element}
          onChange={onChangeWrapper}
      />
  );
};

app.component(name, react2angular(ElementMetaDescriptionAdmin, ['element', 'mode', 'onChange']));

export default name;