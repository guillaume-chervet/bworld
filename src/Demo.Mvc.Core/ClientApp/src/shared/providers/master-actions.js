import {
  MASTER_INIT,
  MASTER_UPDATE,
  MASTER_UPDATE_METAS,
  MASTER_UPDATE_MENU,
} from './master-types';

export const masterInit = data => {
  return {
    type: MASTER_INIT,
    data,
  };
};

export const masterUpdate = data => {
  return {
    type: MASTER_UPDATE,
    data,
  };
};

export const masterUpdateMetas = data => {
  return {
    type: MASTER_UPDATE_METAS,
    data,
  };
};

export const masterUpdateMenu = data => {
  return {
    type: MASTER_UPDATE_MENU,
    data,
  };
};
