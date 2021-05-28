import app from '../../app.module';
import { service as linkService } from './elementLink-factory';
import {ElementAdmin} from "../elementAdmin-component";
import {react2angular} from "react2angular";
import React from "react";
import {Link} from "./elementLink-component";

const name = 'elementLinkAdmin';

const SubComponent = ({element, onChange, menuItems}) => {
  switch (element.type) {
    case 'phone':
      return <div className="form-group">
        <label htmlFor="Label" className="col-sm-3 col-md-3 col-xs-12 control-label">Numéro téléphone</label>
        <div className="col-sm-9 col-md-9 col-xs-12">
          <input type="text" name="field" id="Label" value={element.data.url} onChange={e => onChange(e, "url")} className="form-control"/>
        </div>
      </div>;
    case 'mail':
      return  <div className="form-group">
        <label htmlFor="Url" className="col-sm-3 col-md-3 col-xs-12 control-label">Email</label>
        <div className="col-sm-9 col-md-9 col-xs-12">
          <input type="text" name="field" id="Url" value={element.data.url}  onChange={e => onChange(e, "url")} className="form-control"/>
        </div>
      </div>;
    case 'facebook':
      return  <div className="form-group">
        <label htmlFor="Url" className="col-sm-3 col-md-3 col-xs-12 control-label">Url</label>
        <div className="col-sm-9 col-md-9 col-xs-12">
          <input type="text" name="field" id="Url" value={element.data.url}  onChange={e => onChange(e, "url")} className="form-control"/>
        </div>
      </div>;
    default:
      return <div className="form-group">
        <label htmlFor="Link" className="col-sm-3 col-md-3 col-xs-12 control-label">Page</label>
        <div className="col-sm-9 col-md-9 col-xs-12">
          <select id="Link" name="uLink" className="form-control" value={element.data.id} onChange={e => onChange(e, "id")} >
            <option value="">- Sélectionner -</option>
            {menuItems.map(menuItem => <option key={menuItem.id} value={menuItem.id}>{menuItem.title}</option>)}
          </select>
        </div>
      </div>;
  }
}

const LinkAdmin = ({element, onChange}) => {

  const menuItems = linkService.init();
  return (<>
    <div className="form-group">
      <label htmlFor={element.property} className="col-sm-3 col-md-3 col-xs-12 control-label">Type</label>
      <div className="col-sm-9 col-md-9 col-xs-12">
        <select name="field" id={element.property} className="form-control" value={element.data.type} onChange={e => onChange(e, "type")}>
          <option value="">Page Interne</option>
          <option value="facebook">Page facebook</option>
          <option value="phone">Numéro de téléphone</option>
          <option value="mail">Email</option>
        </select>
      </div>
    </div>
    <SubComponent element={element} onChange={onChange} menuItems={menuItems} />
    <div className="form-group">
      <label htmlFor="uLabel" className="col-sm-3 col-md-3 col-xs-12 control-label">Libellé</label>
      <div className="col-sm-9 col-md-9 col-xs-12">
        <input type="text" name="field" id="uLabel" value={element.data.label} className="form-control"/>
      </div>
    </div>
   </>)
}

export const ElementLinkAdmin = ({ element, mode, onChange }) => {
  const onChangeWrapper = (e, whichElement) => {
      const newData = {...element.data};
      newData[whichElement] = e.target.value
      onChange({ what: "element-edit", element: {...element, data:newData}});
  };
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Liens'}
          adminEdit={<LinkAdmin
              element={element}
              onChange={onChangeWrapper}
          />}
          adminView={<Link element={element} />}>
      </ElementAdmin>
  );
};