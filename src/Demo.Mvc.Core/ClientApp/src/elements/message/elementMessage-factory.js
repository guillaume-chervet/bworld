function initDataElement(element, destElements) {
  var objectData = JSON.parse(element.data);
  if (objectData instanceof Array) {
    objectData = {
      subjects: objectData,
    };
  }
  element.data = objectData;
  destElements.push(element);
}

const addElement = function(parentElement, guid) {
  const newElement = {
    type: 'message',
    property: 'Titres des messages',
    label: 'Description',
    data: {
      subjects: [],
    },
  };
  return newElement;
};

export const service = {
  initDataElement: initDataElement,
  addElement,
};
