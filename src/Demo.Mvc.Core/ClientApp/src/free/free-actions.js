import { FREE_INIT, FREE_SAVE, FREE_HANDLECHANGE } from './free-types';

export const freeInit = data => {
  return {
    type: FREE_INIT,
    data,
  };
};

export const freeSave = () => ({
  type: FREE_SAVE,
});

export const freeOnChange = data => ({
  type: FREE_HANDLECHANGE,
  data,
});
