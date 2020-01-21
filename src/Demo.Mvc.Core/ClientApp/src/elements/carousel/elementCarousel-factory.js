import { service as elementFileService } from '../file/elementFile-factory';

const addElement = function(parentElement, guid) {
  const newElement = {
    type: 'carousel',
    property: guid.guid(),
    label: 'Carousel',
    data: [],
    config: {
      maxWidth: 800,
      maxHeigth: 400,
    },
    $parent: parentElement,
  };
  return newElement;
};

export const service = {
  addElement,
  initDataElement: elementFileService.initDataElement,
};
