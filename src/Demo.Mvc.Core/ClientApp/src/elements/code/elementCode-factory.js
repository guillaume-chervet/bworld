const addElement = function(parentElement, guid) {
  const newElement = {
    type: 'code',
    property: guid.guid(),
    label: 'File',
    data: {
      files: [
        {
          id: guid.guid(),
          fileName: 'exemple.js',
          mode: 'javascript',
          code: '',
        },
      ],
    },
    $parent: parentElement,
  };

  return newElement;
};

export const service = { addElement };
