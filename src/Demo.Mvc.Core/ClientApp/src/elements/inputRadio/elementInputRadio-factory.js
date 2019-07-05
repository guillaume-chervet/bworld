const addElement = function(parentElement, guid) {
  const newElement = {
    type: 'inputRadio',
    property: guid.guid(),
    label: 'Radio',
    data: {
      question: 'Votre question',
      type: 'single',
      response: null,
      responses: {},
      options: [
        { id: guid.guid(), label: 'Réponse 1' },
        { id: guid.guid(), label: 'Réponse 2' },
      ],
    },
    $parent: parentElement,
  };
  return newElement;
};

export const service = {
  addElement,
};
