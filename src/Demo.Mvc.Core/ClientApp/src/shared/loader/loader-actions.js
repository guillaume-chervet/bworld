import { LOADER_ADD, LOADER_CLEAR, LOADER_REMOVE } from './loader-types';

export const loaderAdd = message => {
  return {
    type: LOADER_ADD,
    message,
  };
};

export const loaderRemove = () => ({
  type: LOADER_REMOVE,
});

export const loaderClear = () => ({
  type: LOADER_CLEAR,
});
