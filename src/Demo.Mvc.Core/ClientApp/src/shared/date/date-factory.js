import moment from 'moment';

// Converti une date String internationale en objet Date local
const dateTimeReviver = function(value) {
  let a;
  if (typeof value === 'string') {
    a = /^([0-9]+)-([0-9]+)-([0-9]+)T.*/.exec(value);
    if (a) {
      return new Date(value);
    }
  }
  return value;
};

const isSameDateAs = function(date1, date2) {
  return (
    date1.getFullYear() === date2.getFullYear() &&
    date1.getMonth() === date2.getMonth() &&
    date1.getDate() === date2.getDate()
  );
};

export const dateString = function(endTicks) {
  const date1 = new Date(endTicks);
  const momentObj = moment(date1);
  let date = isSameDateAs(date1, new Date())
    ? "aujourd'hui"
    : `le ${momentObj.format('DD/MM/YYYY')}`;
  return `${date} à ${momentObj.format('HH')}h${momentObj.format('mm')}`;
};

export const displayDuration = function(beginTicks, endTicks) {
  return displayDurationFromMillisecond(
    new Date(endTicks).getTime() - new Date(beginTicks).getTime()
  );
};

export const displayDurationFromMillisecond = function(timeSpanmillisecond) {
  const diff = moment.duration(timeSpanmillisecond);
  let h = diff.hours();
  let hString = h ? `${h} heures` : '';
  let m = diff.minutes();
  let mString = m || h ? `${h} minutes et ` : '';
  return `${hString}${mString}${diff.seconds()} secondes`;
};

export const displayDurationFromSecond = function(timespanSecond) {
  return displayDurationFromMillisecond(1000 * timespanSecond);
};

export const convertStringDateToDateObject = function(origin) {
  if (typeof origin === 'string') {
    return dateTimeReviver(origin);
  }
  for (var propertyName in origin) {
    const value = origin[propertyName];
    if (typeof value === 'string') {
      origin[propertyName] = dateTimeReviver(value);
    } else if (typeof value === 'object') {
      convertStringDateToDateObject(value);
    }
  }
  return origin;
};
