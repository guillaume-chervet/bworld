import q from '../../q';
import { guid } from '../../shared/services/guid-factory';
import { master } from '../../shared/providers/master-provider';
import { userData } from '../../user/info/userData-factory';
import formType from './formType';

import _ from 'lodash';

export const canContinue = function(state) {
  if (!state) {
    return false;
  }
  if (state.endTicks) {
    return false;
  }
  if (state.totalTimeMinute) {
    const timeSecond = getTime(state);
    return timeSecond ? true : false;
  }
  return false;
};

export const getTime = function(state) {
  if (!state.totalTimeMinute) {
    return null;
  }
  const timeElaspedSecond = (new Date().getTime() - state.beginTicks) / 1000;
  const timeSecond = Math.round(state.totalTimeMinute * 60 - timeElaspedSecond);
  if (timeSecond > 0) {
    return timeSecond;
  }
  return null;
};

export const formState = {
  noPrevious: 'notPrevious',
  running: 'running',
  finished: 'finished',
  timeUp: 'timeUp',
};

export const getScore = function(state) {
  if (!state) {
    return {
      formState: formState.noPrevious,
    };
  }
  if (state.endTicks) {
    return {
      formState: formState.finished,
      score: state.score,
      total: state.total,
      timeSpan: state.endTicks - state.beginTicks,
      endTicks: state.endTicks,
    };
  }
  if (state.totalTimeMinute) {
    if (getTime(state)) {
      return {
        formState: formState.running,
      };
    }
  }

  return {
    formState: formState.timeUp,
  };
};

const addElement = function(parentElement) {
  const newElement = {
    type: 'form',
    property: guid.guid(),
    label: 'Form',
    childs: [],
    data: {
      type: 'training',
      label:
        'Vous devez répondre aux questions dans le temps imparti. Le test commence dès que vous cliquez sur le bouton démarrer.',
      time: 30,
    },
    $parent: parentElement,
  };

  newElement.childs.push({
    type: 'div',
    property: guid.guid(),
    label: 'Div',
    childs: [],
    data: { label: 'Page 1' },
    $parent: newElement,
  });
  newElement.childs.push({
    type: 'div',
    property: guid.guid(),
    label: 'Div',
    childs: [],
    data: { label: 'Page 2' },
    $parent: newElement,
  });

  (function() {
    const nbChilds = newElement.childs.length;
    for (var i = 0; i < nbChilds; i++) {
      const child = newElement.childs[i];
      child.$parent = newElement;
      child.childs.push({
        type: 'p',
        property: guid.guid(),
        label: 'Text',
        data: '<h2>Titre colonne</h2><p>Contenu colonne</p>',
        $parent: child,
      });
    }
  })();

  return newElement;
};

const addTab = function(parentElement, currentElement) {
  const newElement = {
    type: 'div',
    property: guid.guid(),
    label: 'Div',
    childs: [],
    data: { label: 'Nouvelle page' },
    $parent: parentElement,
  };

  const child = {
    type: 'p',
    property: guid.guid(),
    label: 'Text',
    data: '<h2>Titre colonne</h2><p>Contenu colonne</p>',
    $parent: newElement,
  };
  newElement.childs.push(child);
  if (!currentElement) {
    parentElement.childs.push(newElement);
  } else {
    const index = parentElement.childs.indexOf(currentElement) + 1;
    parentElement.childs.splice(index, 0, newElement);
  }
};

const form = {};
const deferred = q.defer();
deferred.resolve();
let promise = deferred.promise;
function initUserFormData(formElement) {
  return promise.then(function() {
    const formId = formElement.property;
    if (form[formId] !== undefined) {
      delete form[formId];
    }
  });
}

function getFormData(formElement) {
  const formId = formElement.property;
  if (form[formId] === undefined) {
    form[formId] = {
      datas: {},
      state: {
        beginTicks: null,
        endTicks: null,
        score: null,
        total: null,
        totalTime: null,
      },
    };
  }
  return form[formId];
}
function getUserData(element) {
  const formElement = element.$parent.$parent;
  const id = element.property;
  const formData = getFormData(formElement);
  if (formData.datas[id] === undefined) {
    formData.datas[id] = {
      response: null,
      responses: {},
      responseSubmited: null,
      responsesSubmited: null,
      isValid: null,
      showResult: formElement.data.type === formType.training,
    };
  }
  return formData.datas[id];
}

function getUserState(formElement) {
  const formData = getFormData(formElement);
  return formData.state;
}

function saveAsync(formElement) {
  const formId = formElement.property;
  const formData = getFormData(formElement);
  const userState = formData.state;
  const saveTicks = new Date().getTime();
  userState.lastSaveTicks = saveTicks;
  const dataToSend = {
    id: formData.id,
    elementId: formId,
    json: JSON.stringify(formData),
    beginTicks: userState.beginTicks,
    endTicks: userState.endTicks,
  };
  promise = promise.then(function() {
    return userData.saveAsync(dataToSend).then(function(id) {
      if (id) {
        formData.id = id;
      }
    });
  });
}

function getTitle(title) {
  if (!title) {
    return 'Page sans titre';
  }
  return title;
}

class SaveTimer {
  constructor(formElement) {
    this.id = null;
    this.formElement = formElement;
  }
  start() {
    this.id = setInterval(() => {
      saveAsync(this.formElement);
    }, 10000);
  }
  stop() {
    if (this.id) {
      clearInterval(this.id);
      this.id = null;
    }
  }
}

class DisplayTimer {
  constructor(ctrl, $scope) {
    this.ctrl = ctrl;
    this.$scope = $scope;
  }
  initDisplayedTimer(timeSecond) {
    if (timeSecond) {
      this.ctrl.isTimerAlert = false;
      this.$scope.$on('timer-tick', (event, args) => {
        const seconds = args.seconds + args.minutes * 60 + args.hours * 60 * 60;
        if (seconds < 60 * 5) {
          this.ctrl.isTimerAlert = true;
        }
      });
      this.ctrl.countdown = timeSecond;
    }
  }
  start(timeSecond) {
    this.initDisplayedTimer(timeSecond);
    this.$scope.$broadcast('timer-start');
  }
  stop() {
    this.$scope.$broadcast('timer-stop');
  }
}

const getModuleId = function() {
  return master.getModuleId();
};

const getPreviousUserData = function(formElement) {
  const moduleId = getModuleId();
  const elementId = formElement.property;
  const uData = userData.getData(moduleId, elementId);
  if (!uData || !uData.json.state) {
    return null;
  }
  const formData = _.cloneDeep(uData.json);
  return formData;
};

export const service = {
  addElement,
  addTab,
  initUserFormData,
  getFormData,
  getUserData,
  getUserState,
  getPreviousUserData,
  saveAsync,
  getTitle,
  SaveTimer,
  DisplayTimer,
  canContinue,
  getTime,
  getScore,
};
