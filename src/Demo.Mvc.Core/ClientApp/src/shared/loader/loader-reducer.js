import { LOADER_ADD, LOADER_REMOVE, LOADER_CLEAR } from './loader-types';

export const initialState = {
  numberLoader: 0,
  message: 'Chargement...',
  isLoading: false,
};

const loader = (state = initialState, action) => {
  switch (action.type) {
    case LOADER_ADD: {
      const message = action.message ? action.message : initialState.message;
      const items = action.response;
      const numberLoader = state.numberLoader + 1;
      return {
        numberLoader: numberLoader,
        isLoading: numberLoader > 0,
        message,
      };
    }
    case LOADER_REMOVE: {
      const numberLoader = state.numberLoader > 0 ? state.numberLoader - 1 : 0;
      return {
        message: state.message,
        numberLoader: numberLoader,
        isLoading: numberLoader > 0,
      };
    }
    case LOADER_CLEAR:
      return {
        message: state.message,
        numberLoader: 0,
        isLoading: false,
      };
    default:
      return state;
  }
};

export default loader;
