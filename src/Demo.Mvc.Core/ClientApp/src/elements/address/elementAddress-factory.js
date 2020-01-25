import { master } from '../../shared/providers/master-provider';

const addElement = function(parentElement, guid) {
  const data = {
    address: {
      id: guid.guid(),
      street: '',
      streetComplement: '',
      postalCode: '',
      city: '',
      geo: {
        latitude: 50.6257,
        longitude: -356.9259,
      },
      valid: false,
    },
    coordinate: {
      lat: 50.6257,
      lng: -356.9259,
      zoom: 16,
    },
    markers: {
      site: {
        lat: 50.6257,
        lng: -356.9259,
        message: master.master.titleSite,
        focus: true,
        draggable: true,
      },
    },
    defaults: {
      scrollWheelZoom: true,
    },
  };

  const newElement = {
    type: 'address',
    property: guid.guid(),
    label: 'Adresse',
    data: data,
    $parent: parentElement,
  };
  return newElement;
};

export const service = { addElement };
