import app from '../../app.module';
import { breadcrumb } from '../../breadcrumb/breadcrumb-factory';
import { displayDuration } from '../../shared/date/date-factory';
import { service as elementForm, formState } from './elementForm-factory.js';
import formType from './formType.js';
import view from './form.html';

import _ from 'lodash';

const name = 'elementForm';

const steps = {
  introduction: 'introduction',
  work: 'work',
  end: 'end',
};

const introState = {
  noPreviousTraining: 'noPreviousTraining',
  noPreviousExamen: 'noPreviousExamen',
  runningTraining: 'runningTraining',
  runningExamen: 'runningExamen',
  finishedTrainning: 'finishedTrainning',
  finishedExamen: 'finishedExamen',
};

function ElementController($scope) {
  const ctrl = this;

  // $scope.timerRunning = false;
  if (!ctrl.element.data) {
    ctrl.element.data = {};
  }
  //const saveTimer = new elementForm.SaveTimer(ctrl.element);
  const displayTimer = new elementForm.DisplayTimer(ctrl, $scope);
  ctrl.getTitle = elementForm.getTitle;

  const getIntro = function(element) {
    const previousData = elementForm.getPreviousUserData(element);
    if (!previousData) {
      return {
        introState:
          ctrl.element.data.type === formType.training
            ? introState.noPreviousTraining
            : introState.noPreviousExamen,
        time: ctrl.element.data.time,
        label: ctrl.element.data.label,
        type: ctrl.element.data.type,
      };
    }
    const score = elementForm.getScore(previousData.state);
    if (score.formState === formState.running) {
      return {
        introState:
          ctrl.element.data.type === formType.training
            ? introState.runningTraining
            : introState.runningExamen,
      };
    }

    if (element.data.type === formType.training) {
      return {
        introState: introState.finishedTrainning,
        score,
      };
    }

    return {
      introState: introState.finishedExamen,
      score,
    };
  };

  const init = () => {
    elementForm.initUserFormData(ctrl.element).then(function() {
      ctrl.state = elementForm.getUserState(ctrl.element);
      ctrl.state.step = steps.introduction;
      ctrl.state.hasSubmitOnce = false;
      ctrl.state.hasPerfect = false;
      ctrl.state.percentage = 0;
      ctrl.currentElement = ctrl.element.childs[0];
      ctrl.intro = getIntro(ctrl.element);
    });
  };
  init();

  ctrl.isReturnVisible = function(currentElement) {
    const index = ctrl.element.childs.indexOf(currentElement);
    return index > 0;
  };

  ctrl.isNextVisible = function(currentElement) {
    const index = ctrl.element.childs.indexOf(currentElement);
    return index <= ctrl.element.childs.length - 2;
  };

  ctrl.isActive = function(element) {
    return ctrl.currentElement === element;
  };

  ctrl.finish = function() {
    const state = ctrl.state;
    state.endTicks = new Date().getTime();
    state.step = steps.end;
    const data = computeScore(ctrl.element);
    state.score = data.score;
    state.total = data.total;
    //saveTimer.stop();
    displayTimer.stop();
    return elementForm.saveAsync(ctrl.element);
  };

  ctrl.restart = function() {
    init();
  };

  ctrl.navPreviousScreen = function() {
    breadcrumb.navBack({ dm: null });
  };

  ctrl.startWork = function() {
    const state = ctrl.state;
    state.step = steps.work;
    state.beginTicks = new Date().getTime();
    const timeMinute = ctrl.element.data.time;
    state.totalTimeMinute = timeMinute;
    state.type = ctrl.element.data.type;
    displayTimer.start(timeMinute * 60);
    //saveTimer.start();
    return elementForm.saveAsync(ctrl.element);
  };

  ctrl.continueWork = function() {
    const previousData = elementForm.getPreviousUserData(ctrl.element);
    const currentData = elementForm.getFormData(ctrl.element);
    Object.assign(currentData, previousData);
    ctrl.state = currentData.state;

    const timeSecond = elementForm.getTime(currentData.state);
    if (timeSecond) {
      displayTimer.start(timeSecond);
      //saveTimer.start();
    } else {
      ctrl.finish();
    }
  };

  ctrl.navBack = function(currentElement) {
    const newIndex = ctrl.element.childs.indexOf(currentElement) - 1;
    if (newIndex >= 0) {
      ctrl.currentElement = ctrl.element.childs[newIndex];
    }
    ctrl.state.hasSubmitOnce = true;
    ctrl.state.hasPerfect = false;
  };

  function isGoodReponse(model, data) {
    if (data.type === 'single') {
      if (data.response === model.response) {
        return true;
      }
    } else if (data.type === 'multiple' && data.responses && model.responses) {
      const sort = function(item1, item2) {
        return item1.key - item2.key;
      };
      const dataResponses = Object.keys(data.responses)
        .map(function(key) {
          return { key, value: data.responses[key] };
        })
        .filter(e => e.value === true)
        .sort(sort);
      const modelResponses = Object.keys(model.responses)
        .map(function(key) {
          return { key, value: model.responses[key] };
        })
        .filter(e => e.value === true)
        .sort(sort);
      return _.isEqual(dataResponses, modelResponses);
    }
    return false;
  }

  ctrl.submit = function(form, currentElement) {
    ctrl.state.hasPerfect = true;
    currentElement.childs.forEach(function(element) {
      if (element.data && element.data.response) {
        const model = elementForm.getUserData(element);
        if (element.data.type === 'single') {
          model.responseSubmited = element.data.response;
        } else {
          model.responsesSubmited = element.data.responses;
        }
        if (isGoodReponse(model, element.data)) {
          model.isValid = true;
        } else {
          model.isValid = false;
          ctrl.state.hasPerfect = false;
        }
      }
    }, this);
    ctrl.state.hasSubmitOnce = true;
    return elementForm.saveAsync(ctrl.element);
  };

  function isQuestionFilled(hasFilled, data, model) {
    if (data.type === 'single' && data.response) {
      if (model.response === null) {
        return false;
      }
    } else if (data.type === 'multiple' && data.responses) {
      if (!model.responses) {
        return false;
      }
      for (var name in model.responses) {
        if (model.responses[name]) {
          return hasFilled;
        }
      }
      return false;
    }
    return hasFilled;
  }

  ctrl.isSubmitDisabled = function(currentElement) {
    let hasFilled = true;
    currentElement.childs.forEach(function(element) {
      if (element.type === 'radio' && element.data) {
        const model = elementForm.getUserData(element);
        hasFilled = isQuestionFilled(hasFilled, element.data, model);
      }
    }, this);
    return !hasFilled || ctrl.state.hasPerfect;
  };

  function isQuestionConfigured(element) {
    if (!element && element.type === 'radio' && element.data) {
      return false;
    }
    const data = element.data;
    if (data.type === 'single' && data.response) {
      if (data.response) {
        return true;
      }
    } else if (data.type === 'multiple' && data.responses) {
      for (var name in data.responses) {
        if (data.responses[name]) {
          return true;
        }
      }
    }

    return false;
  }

  const computePercentage = function(formElement, currentElement) {
    let total = 0;
    formElement.childs.forEach(function(parentElement) {
      //total = total + parentElement.childs.length;
      parentElement.childs.forEach(function(element) {
        if (isQuestionConfigured(element)) {
          total++;
        }
      });
    });

    const index = formElement.childs.indexOf(currentElement);

    let current = 0;
    for (let i = 0; i <= index; i++) {
      const parentElement = formElement.childs[i];
      //current = current + parentElement.childs.length;
      parentElement.childs.forEach(function(element) {
        if (isQuestionConfigured(element)) {
          current++;
        }
      });
    }

    if (total < 1) {
      return 0;
    }

    const percentage = Math.ceil((current / total) * 100);
    return percentage;
  };

  const computeScore = function(formElement) {
    let total = 0;
    let score = 0;
    formElement.childs.forEach(function(parentElement) {
      parentElement.childs.forEach(function(element) {
        if (isQuestionConfigured(element)) {
          total++;
          let model = elementForm.getUserData(element);
          if (isGoodReponse(model, element.data)) {
            score++;
          }
        }
      }, this);
    });
    const time = displayDuration(ctrl.state.beginTicks, ctrl.state.endTicks);
    return { total, score, time };
  };

  ctrl.computeScore = () => computeScore(ctrl.element);

  ctrl.next = function(form, currentElement) {
    const newIndex = ctrl.element.childs.indexOf(currentElement) + 1;
    const length = ctrl.element.childs.length;
    if (length > newIndex) {
      ctrl.currentElement = ctrl.element.childs[newIndex];
    }
    ctrl.state.hasPerfect = false;
    ctrl.state.hasSubmitOnce = false;
    ctrl.state.percentage = computePercentage(ctrl.element, currentElement);
  };

  return ctrl;
}

app.component(name, {
  template: view,
  controller: ['$scope', ElementController],
  bindings: {
    element: '=',
  },
});

export default name;
