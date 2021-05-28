import { service as elementGridElementService } from './elementGridElement-factory';
import {ElementAdmin} from "../elementAdmin-component";
import React from "react";
import {GridElement} from "./elementGridElement-component";
import {DivAdmin} from "../div/elementDivAdmin-component";

const AdminContent = ({element, onChange,onChangeProperty}) => {

  if(element.data.type.toString() === "2"){
    return (<div className="panel panel-default">
      <div className="panel-heading">
        <div ng-form name="fieldForm" className="form-group">
          <label htmlFor="title" className="col-sm-12 col-md-12 col-xs-12 control-label">Titre en-tête</label>
          <div className="col-sm-12 col-md-12 col-xs-12">
            <input type="text" name="field" id="title" value={element.data.title} onChange={(e) => onChangeProperty(e,"title")}
                   className="form-control"/>
          </div>
        </div>
      </div>
      <div className="panel-body">
        <DivAdmin element={element} onChange={onChange}>
        </DivAdmin>
      </div>
      <div className="panel-footer">
        <div className="form-group">
          <label htmlFor="footer" className="col-sm-12 col-md-12 col-xs-12 control-label">Titre pied</label>
          <div className="col-sm-12 col-md-12 col-xs-12">
            <input type="text" name="field" id="footer" value={element.data.footer}
                   className="form-control" onChange={(e) => onChangeProperty(e,"footer")}/>
          </div>
        </div>
      </div>
    </div>)
  }
 return (
   <DivAdmin element={element} onChange={onChange}/>);
}

const Admin = ({element, onChange}) => {
  const onChangeWrapper = (data) => {
    onChange({ what: "element-edit", element: {...element, data}});
  };
  
  if(!element.data){
    element.data = {type:"1"};
  }
  
  const onChangeProperty = (e, propertyName) => {
    const newData = {...element.data};
    newData[propertyName] = e.target.value;
    onChangeWrapper(newData);
  }

 return (<div className="row mw-grid-element">
    <div className="form-group">
      <label htmlFor="Type" className="col-sm-12 col-md-12 col-xs-12 control-label">Type affichage</label>
      <div className="col-sm-12 col-md-12 col-xs-12">
        <select id="Type" name="uType" className="form-control" value={element.data.type} onChange={(e) => onChangeProperty(e,"type")}>
          <option value="">- Sélectionner -</option>
          <option value="1">Colonne</option>
          <option value="2">Panel</option>
        </select>
      </div>
    </div>
   <AdminContent element={element} onChange={onChange} onChangeProperty={onChangeProperty}/>
  </div>);
}

export const ElementGridElementAdmin = ({ element, mode, onChange }) => {
  const cssClass = elementGridElementService.cssClass;

  
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Texte'}
          adminEdit={<Admin
              element={element}
              onChange={onChange}

          />}
          adminView={<GridElement element={element} />}>
      </ElementAdmin>
  );
};
