const addElement = function(parentElement, guid) {
  const newElement = {
    type: 'div',
    property: guid.guid(),
    label: 'Div',
    childs: [],
    $parent: parentElement,
  };

  return newElement;
};

export const service = {
  addElement,
};
