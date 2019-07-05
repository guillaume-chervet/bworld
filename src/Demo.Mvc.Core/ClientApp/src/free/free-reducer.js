import { FREE_INIT, FREE_SAVE, FREE_HANDLECHANGE } from './free-types';
import _ from 'lodash';

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
      const newElement = action.element;
      const element = findElement(state.element, newElement.property);
      element.data = newElement.data;
      return {
        ...state,
      };
    }
    default:
      return state;
  }
};

export default free;
