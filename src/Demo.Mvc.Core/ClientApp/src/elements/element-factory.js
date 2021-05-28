import {guid} from '../shared/services/guid-factory';
import {convertStringDateToDateObject} from '../shared/date/date-factory';
import services from './services';

import _ from 'lodash';

const addElement = (type, parentElement) => {
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
      const nbChilds = newElement.childs.length;
      for (let i = 0; i < nbChilds; i++) {
        const child = newElement.childs[i];
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

  const index = _parent.childs.indexOf(parentElement) + 1;
  newElement.$parent = _parent;
  if (_parent.$level !== undefined) {
    newElement.$level = _parent.$level + 1;
  } else {
    newElement.$level = 0;
  }
  _parent.childs.splice(index, 0, newElement);
};

const _getService =(type) => {
  if (!type) {
    return null;
  }
  return services[type];
};

const initDataElement=(
  moduleId,
  sourceElements,
  destElements,
  destMetaElements
) => {
  sourceElements.forEach(element => {
    try {
      const _service = _getService(element.type);

      if(!element.property){
        element.property= guid.guid();
      }

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
};

const mapElement = (elements, metaElements) => {
  if (!elements) {
    return null;
  }
  const elementsTemp = [];
  elements.forEach((element) => {
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
        childs,
      });
    } else {
      elementsTemp.push({
        type: element.type,
        property: element.property,
        data: element.data,
        childs,
      });
    }
  });

  if (metaElements) {
    metaElements.forEach(metaElement => {
      elementsTemp.push({
        type: metaElement.type,
        property: metaElement.property,
        data: metaElement.data,
        childs: null,
      });
    });
  }
  return elementsTemp;
};

export const defaultState = { isEdit : false, hover : false, focus : false};

const inherit = (ctrl = {}, e, state = defaultState , setState) => {
  const element =  e || ctrl.element;
  
  if(!setState){
    ctrl.isEdit = false;
    ctrl.hover = false;
    ctrl.focus = false;
    
    setState = newState => {
      ctrl.isEdit = newState.isEdit;
      ctrl.hover = newState.hover;
      ctrl.focus = newState.focus;
      state.isEdit = newState.isEdit;
      state.hover = newState.hover;
      state.focus = newState.focus;
    }
  }

  const initEditMode = () => {
    if (element) {
      const data = element.data;

      if (element.type === 'hr') {
        return false;
      } else if (
        element &&
        element.childs &&
        element.childs.length > 0
      ) {
        return false;
      } else if (_.isArray(data)) {
        if (data.length <= 0) {
          return true;
        }
      } else if (!data) {
        return true;
      } else if (element.type === 'code') {
        return data.files.length <= 0 || !data.files[0].code;
      }
    }
    return false;
  };

  const isEditMode = () => {
    if (initEditMode()) {
      return true;
    }
    return state.isEdit;
  };
  const clickEdit = () => {
    setState( {...state, isEdit: !state.isEdit});
  };

  /*if (ctrl.isEdit === undefined) {
    setState( {...state, isEdit: !initEditMode()});
  }*/

  ctrl.addElement = (type) => {
    element.addElement(type, ctrl);
  };

  ctrl.isLastElement = elem => {
    let _childs = null;
    if (!elem) {
      elem = element;
      _childs = elem.$parent.childs;
    } else {
      _childs = elem.$parent.childs;
    }
    return _childs.indexOf(elem) >= _childs.length - 1;
    
  };

  ctrl.isEditButtonDisabled = () => {
    if (element) {
      const data = element.data;
      if (Array.isArray(data)) {
        if (data.length <= 0) {
          return true;
        }
      } else if (data === undefined) {
        return null;
      } else if (!data) {
        return true;
      } else if (element.type === 'code') {
        return data.files.length <= 0 || !data.files[0].code;
      }
    }
    return false;
  };

  const isEditButton = () => {
    return state.hover || isEditMode();
  };

  const select = () => {
    setState( {...state, hover: true});
  };
  const unselect = () => {
    setState( {...state, hover: false});
  };
  const isSelect = () => {
    return state.hover;
  };

  ctrl.unselect = unselect;
  ctrl.select = select;
  ctrl.isSelect = isSelect;

  const doFocus = () => {
    setState({...state, focus: true});
  };
  const doUnfocus = () => {
    setState({...state, focus: false});
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
  return ctrl;
};

export const service = {
  mapElement,
  initDataElement,
  addElement,
  inherit,
};
