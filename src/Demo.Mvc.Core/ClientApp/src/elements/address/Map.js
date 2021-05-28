import React from 'react';
import { MapContainer, Marker, Popup, TileLayer } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';

const MwMap = props => {
  const { element } = props;
  const coordinate = element.data.coordinate;
  const position = [coordinate.lat, coordinate.lng];
  const siteCoordinate = element.data.markers.site;
  const sitePosition = [siteCoordinate.lat, siteCoordinate.lng];
  return (
    <MapContainer center={position} zoom={coordinate.zoom}>
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
      />
      <Marker position={sitePosition}>
        <Popup>{element.data.markers.site.message}</Popup>
      </Marker>
    </MapContainer>
  );
};

export default MwMap;
