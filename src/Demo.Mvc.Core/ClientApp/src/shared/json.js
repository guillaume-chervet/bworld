import { convertStringDateToDateObject } from './date/date-factory';

const parse = function(json) {
  if (!json) {
    return null;
  }
  const object = JSON.parse(json);
  return convertStringDateToDateObject(object);
};

export default { parse };
