import { guid } from '../shared/services/guid-factory';
import { convertStringDateToDateObject } from '../shared/date/date-factory';
import services from './services';

import _ from 'lodash';

var $ = window.$;

const addElement = function(type, parentElement) {
  let newElement;
  let _parent = null;
  const _service = _getService(type);
  if (_service != null && _service.addElement) {
    newElement = _service.addElement(parentElement, guid);
  } else if (type === 'gridElement') {
    _parent = parentElement;
    newElement = {
      type: type,
      property: guid.guid(),
      label: 'GridElement',
      childs: [
        {
          type: 'p',
          property: guid.guid(),
          label: 'Contenu',
          data: '<h2>Titre colonne</h2><p>Contenu colonne</p>',
          $parent: null,
        },
      ],
      $parent: parentElement,
    };
    (function() {
      var nbChilds = newElement.childs.length;
      for (var i = 0; i < nbChilds; i++) {
        var child = newElement.childs[i];
        child.$parent = newElement;
      }
    })();
  } else {
    newElement = {
      type: type,
      property: guid.guid(),
      label: 'Text',
      data: '',
      $parent: parentElement,
    };
  }

  if (_parent === null) {
    _parent = parentElement.$parent;
  }

  var index = _parent.childs.indexOf(parentElement) + 1;
  newElement.$parent = _parent;
  if (_parent.$level !== undefined) {
    newElement.$level = _parent.$level + 1;
  } else {
    newElement.$level = 0;
  }
  _parent.childs.splice(index, 0, newElement);
};

function _getService(type) {
  if (!type) {
    return null;
  }
  const _service = services[type];
  return _service;
}

function initDataElement(
  moduleId,
  sourceElements,
  destElements,
  destMetaElements
) {
  sourceElements.forEach(function(element) {
    try {
      const _service = _getService(element.type);

      if (_service && _service.initDataElement) {
        _service.initDataElement(
          element,
          destElements,
          moduleId,
          destMetaElements
        );
        return;
      }

      let _data = JSON.parse(element.data);
      if (_data) {
        _data = convertStringDateToDateObject(_data);
      }
      element.data = _data;
      if (element.childs && element.childs.length > 0) {
        const childElements = [];
        initDataElement(
          moduleId,
          element.childs,
          childElements,
          destMetaElements
        );
        element.childs = childElements;
      }
      destElements.push(element);
    } catch (exception) {
      console.log('initDataElement ' + exception.message + element.data);
    }
  });
}

function mapElement(elements, metaElements) {
  if (!elements) {
    return null;
  }
  const elementsTemp = [];
  elements.forEach(function(element) {
    let childs = null;
    if (element.childs) {
      childs = mapElement(element.childs);
    }
    if (
      element.type === 'file' ||
      element.type === 'image' ||
      element.type === 'carousel'
    ) {
      const clientFiles = element.data;
      const serveurFiles = [];

      clientFiles.forEach(function(clientFile) {
        serveurFiles.push({
          id: clientFile.id,
          propertyName: clientFile.propertyName,
          name: clientFile.name,
          type: clientFile.type,
          size: clientFile.size,
          title: clientFile.title,
          description: clientFile.description,
          behavior: clientFile.behavior,
          thumbDisplayMode: clientFile.thumbDisplayMode,
          isTemporary: clientFile.isTemporary,
          link: clientFile.link,
        });
      }, this);
      elementsTemp.push({
        type: element.type,
        property: element.property,
        data: JSON.stringify(serveurFiles),
      });
    } else if (typeof element.data !== 'string') {
      elementsTemp.push({
        type: element.type,
        property: element.property,
        data: JSON.stringify(element.data),
        childs: childs,
      });
    } else {
      elementsTemp.push({
        type: element.type,
        property: element.property,
        data: element.data,
        childs: childs,
      });
    }
  });

  if (metaElements) {
    metaElements.forEach(function(metaElement) {
      elementsTemp.push({
        type: metaElement.type,
        property: metaElement.property,
        data: metaElement.data,
        childs: null,
      });
    });
  }
  return elementsTemp;
}

const inherit = (ctrl) => {
  ctrl.isEdit = false;
  const initEditMode = () => {
    if (ctrl.element) {
      const data = ctrl.element.data;

      if (ctrl.element.type === 'hr') {
        return false;
      } else if (
        ctrl.element &&
        ctrl.element.childs &&
        ctrl.element.childs.length > 0
      ) {
        return false;
      } else if (_.isArray(data)) {
        if (data.length <= 0) {
          return true;
        }
      } else if (!data) {
        return true;
      } else if (ctrl.element.type === 'code') {
        return data.files.length <= 0 || !data.files[0].code;
      }
    }
    return false;
  };

  const isEditMode = () => {
    if (initEditMode()) {
      return true;
    }
    return ctrl.isEdit;
  };
  const clickEdit = () => {
    ctrl.isEdit = !ctrl.isEdit;
  };

  if (ctrl.isEdit === undefined) {
    ctrl.isEdit = initEditMode();
  }

  ctrl.addElement = function(type) {
    ctrl.element.addElement(type, ctrl);
  };

  ctrl.isLastElement = function(elem) {
    let _childs = null;
    if (!elem) {
      elem = ctrl.element;
      _childs = elem.$parent.childs;
    } else {
      _childs = elem.$parent.childs;
    }
    if (_childs.indexOf(elem) >= _childs.length - 1) {
      return true;
    }
    return false;
  };

  ctrl.isEditButtonDisabled = () => {
    if (ctrl.element) {
      const data = ctrl.element.data;
      if ($.isArray(data)) {
        if (data.length <= 0) {
          return true;
        }
      } else if (data === undefined) {
        return null;
      } else if (!data) {
        return true;
      } else if (ctrl.element.type === 'code') {
        return data.files.length <= 0 || !data.files[0].code;
      }
    }
    return false;
  };

  const isEditButton = () => {
    return ctrl.hover || isEditMode();
  };

  ctrl.hover = false;
  const select = () => {
    ctrl.hover = true;
  };
  const unselect = () => {
    ctrl.hover = false;
  };
  const isSelect = () => {
    return ctrl.hover;
  };

  ctrl.unselect = unselect;
  ctrl.select = select;
  ctrl.isSelect = isSelect;
  ctrl.focus = false;
  const doFocus = () => {
    ctrl.focus = true;
  };
  const doUnfocus = () => {
    ctrl.focus = false;
  };
  const isFocus = () => {
    return ctrl.focus;
  };

  ctrl.doUnfocus = doUnfocus;
  ctrl.doFocus = doFocus;
  ctrl.isFocus = isFocus;
  ctrl.isEditMode = isEditMode;
  ctrl.clickEdit = clickEdit;
  ctrl.isEditButton = isEditButton;
};

export const service = {
  mapElement: mapElement,
  initDataElement: initDataElement,
  addElement: addElement,
  inherit: inherit,
};
