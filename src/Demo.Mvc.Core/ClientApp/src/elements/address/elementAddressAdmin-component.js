import _ from 'lodash';

import app from '../../app.module';
import { guid } from '../../shared/services/guid-factory';
import { master } from '../../shared/providers/master-provider';
import $http from '../../http';
import {ElementAdmin} from "../elementAdmin-component";
import {react2angular} from "react2angular";
import React from "react";
import {Address} from "./elementAddress-component";

const name = 'elementAddressAdmin';

const AddressAdmin = ({element, ctrl, onChange}) => {
  const data =  _.cloneDeep(element.data)
  const onChangeAdresse = (e, name) => { 
    data.address[name] = e.target.value; 
    onChange(data);
  }
  
  return (<><div className="form-group">
    <label htmlFor="Label" className="col-sm-3 col-md-3 col-xs-12 control-label">Libellé</label>
    <div className="col-sm-9 col-md-9 col-xs-12">
      <input type="text" name="field" id="Label" value={data.markers.site.message} onChange={(e)=> { data.markers.site.message = e.target.value; onChange(data);}} className="form-control"/>
    </div>
  </div>
  <div className="form-group">
    <label htmlFor="Street" className="col-sm-3 col-md-3 col-xs-12 control-label">Rue</label>
    <div className="col-sm-9 col-md-9 col-xs-12">
      <input type="text" name="field" id="Street" value={data.address.street} onChange={(e)=> onChangeAdresse(e,"street")} className="form-control"/>
    </div>
  </div>
  <div className="form-group">
    <label htmlFor="Complement" className="col-sm-3 col-md-3 col-xs-12 control-label">Complément adresse</label>
    <div className="col-sm-9 col-md-9 col-xs-12">
      <input type="text" name="field" id="Complement" value={data.address.streetComplement} onChange={(e)=> onChangeAdresse(e,"streetComplement")} className="form-control"/>
    </div>
    
  </div>
  <div className="form-group">
    <label htmlFor="PostalCode" className="col-sm-3 col-md-3 col-xs-12 control-label">Code postal</label>
    <div className="col-sm-9 col-md-9 col-xs-12">
      <input type="text" name="field" id="PostalCode" value={data.address.postalCode} onChange={(e)=> onChangeAdresse(e,"postalCode")} className="form-control"/>
    </div>
  </div>
  <div className="form-group">
    <label htmlFor="City" className="col-sm-3 col-md-3 col-xs-12 control-label">Ville</label>
    <div className="col-sm-9 col-md-9 col-xs-12">
      <input type="text" name="field" id="City" value={data.address.city}  onChange={(e)=> onChangeAdresse(e,"city")} className="form-control"/>
    </div>
  </div>

  <div className="form-group">
    <div className="col-sm-3 col-xs-6">
    </div>
    <div className="col-sm-9 col-xs-6 mw-action">
      <button type="button" className="btn btn-default" onClick={ctrl.geolocalize}>
        <span className="glyphicon glyphicon-search"/><span>Valider adresse et centrer sur la carte</span>
      </button>
      <p>
        Latitude: {data.markers.site.lat}
        Longitude {data.markers.site.lng}
        <span ><img src={data.address.valid ? "Content/images/p_ok.png": "Content/images/p_ko.png"} alt="ok"/></span>
      </p>
    </div>
  </div>

  <div className="clearfix"/>

  <div className="row">
    <div className="col-sm-1 col-md-1 col-xs-1"/>
    <div className="col-sm-10 col-md-10 col-xs-10">
      <Address style="height: 480px;" element={element} />
    </div>
    <div className="col-sm-1 col-md-1 col-xs-1"/>
  </div></>);
}

export const ElementAddressAdmin = ({ element, mode, onChange }) => {


  const _element = element;

  if (!_element.data.address.id) {
    _element.data.address.id = guid.guid();
  }

  ctrl.invalidAddress = () => {
    _element.data.address.valid = false;
  };

  ctrl.markers = {};
  Object.assign(ctrl.markers, _.cloneDeep(_element.data.markers));
  ctrl.markers.site.draggable = true;

  ctrl.events = {
    markers: {
      enable: ['dragend'],
    },
  };

  ctrl.geolocalize = () => {
    const dataToSend = {
      street: _element.data.address.street,
      postalCode: _element.data.address.postalCode,
      city: _element.data.address.city,
    };

    return $http
        .post(master.getUrl('api/geo/post'), dataToSend, {
          headers: {
            loaderMessage: 'Recherche en cours...',
          },
        })
        .then(function(response) {
          if (response.data.isSuccess) {
            const result = response.data.data;

            if (result.length > 0) {
              const geo = result[0];

              const _nLat = Number(geo.lat);
              const _nLon = Number(geo.lon);
              _element.data.address.valid = true;
              _element.data.address.geo = {
                latitude: _nLat,
                longitude: _nLon,
              };

              _element.data.coordinate.lat = _nLat;
              _element.data.coordinate.lng = _nLon;

              _element.data.markers.site.lat = _nLat;
              _element.data.markers.site.lng = _nLon;
            }
          }
        });
  };
  
  const onChangeWrapper = (data) => {
    onChange({ what: "element-edit", element: {...element, data}});
  };
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Adresse'}
          adminEdit={<AddressAdmin
              element={element}
              ctrl={ctrl}
              onChange={onChangeWrapper}
          />}
          adminView={<Address element={element} />}>
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementAddressAdmin, ['element', 'mode', 'onChange']));

export default name;
