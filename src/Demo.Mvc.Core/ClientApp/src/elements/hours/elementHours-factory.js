import { guid } from '../../shared/services/guid-factory';

const _clone = function(oldObject) {
  return JSON.parse(JSON.stringify(oldObject));
};

function addElement(parentElement) {
  const beginDate = new Date();
  beginDate.setHours(8);
  beginDate.setMinutes(0);

  const endDate = new Date();
  endDate.setHours(18);
  endDate.setMinutes(30);

  const monday = {
    isOpen: true,
    label: 'Lundi',
    hours: [
      {
        begin: beginDate,
        end: endDate,
      },
    ],
  };
  const tuesday = _clone(monday);
  tuesday.label = 'Mardi';
  const wednesday = _clone(monday);
  wednesday.label = 'Mercredi';
  const thursday = _clone(monday);
  thursday.label = 'Jeudi';
  const friday = _clone(monday);
  friday.label = 'Vendredi';
  const saturday = _clone(monday);
  saturday.label = 'Samedi';
  const sunday = _clone(monday);
  sunday.label = 'Dimanche';
  sunday.isOpen = false;

  const days = [monday, tuesday, wednesday, thursday, friday, saturday, sunday];

  const newElement = {
    type: 'hours',
    property: guid.guid(),
    label: 'Lien',
    data: {
      days: days,
    },
    $parent: parentElement,
  };
  return newElement;
}

export const service = {
  addElement: addElement,
};
