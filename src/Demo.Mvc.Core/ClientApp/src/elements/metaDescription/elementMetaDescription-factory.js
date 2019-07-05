var getElement = function(elements, propertyName) {
  for (var i = 0; i < elements.length; i++) {
    if (elements[i].property === propertyName) {
      return elements[i];
    }
  }
  return null;
};

function initDataElement(element, destElements, moduleId, destMetaElements) {
  //if (element.property === 'MetaDescription') {
  var metaElem = getElement(destMetaElements, 'MetaDescription');
  if (metaElem) {
    metaElem.data = element.data;
  } else {
    destMetaElements.push(element);
  }
  //}
}

export const service = {
  initDataElement: initDataElement,
};
