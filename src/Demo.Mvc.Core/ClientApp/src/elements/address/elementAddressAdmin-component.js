import _ from 'lodash';

import app from '../../app.module';
import { service as elementService } from '../element-factory';
import { guid } from '../../shared/services/guid-factory';
import { master } from '../../shared/providers/master-provider';
import $http from '../../http';
import view from './address_admin.html';

const name = 'elementAddressAdmin';

class Controller {
  $onInit() {
    var ctrl = this;

    var _element = ctrl.element;

    if (!_element.data.address.id) {
      _element.data.address.id = guid.guid();
    }

    ctrl.invalidAddress = function() {
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

    /* $scope.$on('leafletDirectiveMarker.dragend', function (event, args) {
        _element.data.markers.site.lat = args.model.lat;
        _element.data.markers.site.lng = args.model.lng;
    });
*/
    ctrl.geolocalize = function() {
      var dataToSend = {
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
            var result = response.data.data;

            if (result.length > 0) {
              var geo = result[0];

              var _nLat = Number(geo.lat);
              var _nLon = Number(geo.lon);
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

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;
