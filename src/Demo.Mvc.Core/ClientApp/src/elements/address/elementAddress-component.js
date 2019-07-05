import app from '../../app.module';
import history from '../../history';
import { master } from '../../shared/providers/master-provider';

import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import LoadableMap from './LoadableMap';

import './address.css';

const name = 'elementAddress';

const getLdJson = function(element, master, history) {
  if (!element || !element.data) {
    return '';
  }
  const _address = element.data.address;
    if (!_address.id || !_address.geo) {
    return '';
  }
  const url = master.getFullUrl(history.path());
  const localBusinessJson = {
    '@context': 'http://schema.org',
    '@type': 'LocalBusiness',
    name: element.data.markers.site.message,
    '@id': url + '#map=' + _address.id,
    address: {
      streetAddress: _address.street,
      addressLocality: _address.city,
      postalCode: _address.postalCode,
      addressCountry: 'FR',
    },
    url: url,
    geo: {
        latitude: _address.geo.latitude,
      longitude: _address.geo.longitude,
    },
  };
  return localBusinessJson;
};

const MapLevel = ({ element }) => {
  if (element.$level <= 2) {
    return (
      <div className="row">
        <div className="col-sm-1 col-md-1 col-xs-1" />
        <div className="col-sm-10 col-md-10 col-xs-10">
          <LoadableMap style="height: 480px;" element={element} />
        </div>
        <div className="col-sm-1 col-md-1 col-xs-1" />
      </div>
    );
  }
  return (
    <div>
      <LoadableMap style="height: 280px;" element={element} />
    </div>
  );
};

const JsonLd = ({ data }) => (
  <script
    type="application/ld+json"
    dangerouslySetInnerHTML={{ __html: JSON.stringify(data) }}
  />
);

const AddressText = ({ element }) => {
  const _address = element.data.address;
  if (!_address.id) {
    return null;
  }

  return (
    <p>
      {element.data.markers.site.message}, {_address.street} {_address.city},{' '}
      {_address.postalCode}, France <br />
    </p>
  );
};

const Address = props => {
  const { element } = props;
  const jsonLd = getLdJson(element, master, history);
  return (
    <div className="mw-address">
      <AddressText element={element} />
      <JsonLd data={jsonLd} />
      <MapLevel element={element} />
    </div>
  );
};

app.component(name, react2angular(Address, ['element']));
