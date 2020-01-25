const addElement = function(parentElement, guid) {
  const newElement = {
    type: 'hr',
    property: guid.guid(),
    label: 'Link',
    data: { type: '' },
    $parent: parentElement,
  };
  return newElement;
};

export const service = { addElement };
