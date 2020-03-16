import { FREE_INIT, FREE_SAVE, FREE_HANDLECHANGE } from './free-types';
import { menu } from '../elements/elementMenu-factory';
import {service as elementService} from "../elements/element-factory";

export const initialState = {
  element: {},
  metaElement: {},
  data: {
    userInfo: null,
    lastUpdateUserInfo: null,
    isDisplayAuthor: null,
    isDisplaySocial: null,
    createDate: null,
    updateDate: null,
  },
};

const findElement = (element, property) => {
  if (!element) {
    return null;
  }
  if (element.property === property) {
    return element;
  }
  const childs = element.childs;
  if (childs) {
    for (let c of childs) {
      const e = findElement(c, property);
      if (e) {
        return e;
      }
    }
  }
  return null;
};

const free = (state = initialState, action) => {
  switch (action.type) {
    case FREE_INIT: {
      const data = action.data;
      return {
        element: data.element,
        metaElement: data.metaElement,
        data: data.data,
      };
    }
    case FREE_HANDLECHANGE: {
      switch (action.data.what){
        case "element-edit" : {
          const newElement = action.data.element;
          const element = findElement(state.element, newElement.property);
          element.data = newElement.data;
          return {
            ...state,
          };   
        }
        case "element-up" : {
          const newElement = action.data.element;
          const element = findElement(state.element, newElement.property);
          menu.up(element, element.$parent);
          return {
            ...state,
          };
        }
        case "element-down" : {
          const newElement = action.data.element;
          const element = findElement(state.element, newElement.property);
          menu.down(element, element.$parent);
          return {
            ...state,
          };
        }
        case "element-delete" : {
          const newElement = action.data.element;
          const element = findElement(state.element, newElement.property);
          menu.deleteElement(element, element.$parent);
          return {
            ...state,
          };
        }
        case "element-add" : {
          elementService.addElement(action.data.selectedItem, action.data.element);
          return {
            ...state,
          };
        }
        
        default:
          return state;
      }
    }
    default:
      return state;
  }
};

export default free;
