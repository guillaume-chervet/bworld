import app from '../../app.module';
import {ElementAdmin} from "../elementAdmin-component";
import {react2angular} from "react2angular";
import React from "react";
import {YouTubeComponent} from "./elementYoutube-component";

import "./youtube.css";

const name = 'elementYoutubeAdmin';

const YouTubeAdmin = ({element, onChange}) => {
  return (<div className="form-group">
    <label htmlFor="Label" className="col-sm-3 col-md-3 col-xs-12 control-label">Url *</label>
    <div className="col-sm-9 col-md-9 col-xs-12">
      <input type="text" name="field" id="Label" value={element.data.url} onChange={onChange} className="form-control"/>
    </div>
  </div>)
}

export const ElementHRAdmin = ({ element, mode, onChange }) => {
  const onChangeWrapper = (e) => {
    onChange({ what: "element-edit", element: {...element, data: {url: e.target.value}}});
  };
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Vidéo Youtube'}
          adminEdit={<YouTubeAdmin
              element={element}
              onChange={onChangeWrapper}
          />}
          adminView={<YouTubeComponent element={element} />}>
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementHRAdmin, ['element', 'mode', 'onChange']));

export default name;
