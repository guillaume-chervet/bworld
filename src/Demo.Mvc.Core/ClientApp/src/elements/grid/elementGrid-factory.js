const addElement = function(parentElement, guid) {
  const newElement = {
    type: 'grid',
    property: guid.guid(),
    label: 'Grid',
    childs: [
      {
        type: 'gridElement',
        property: guid.guid(),
        label: 'GridElement',
        childs: [],
      },
      {
        type: 'gridElement',
        property: guid.guid(),
        label: 'GridElement',
        childs: [],
      },
    ],
    $parent: parentElement,
  };
  (function() {
    var nbChilds = newElement.childs.length;
    for (var i = 0; i < nbChilds; i++) {
      var child = newElement.childs[i];
      child.$parent = newElement;
      child.childs.push({
        type: 'p',
        property: guid.guid(),
        label: 'Text',
        data: '<h2>Titre colonne</h2><p>Contenu colonne</p>',
        $parent: child,
      });
    }
  })();
  return newElement;
};

export const service = { addElement };
