import { USER_INIT, USER_UPDATE, USER_LOGOFF } from './user-types';

export const userInit = data => {
  return {
    type: USER_INIT,
    data,
  };
};

export const userUpdate = data => {
  return {
    type: USER_UPDATE,
    data,
  };
};

export const userLogOff = data => {
  return {
    type: USER_LOGOFF,
    data,
  };
};
