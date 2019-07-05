import app from '../../app.module';
import {
  displayDuration,
  dateString,
  displayDurationFromMillisecond,
  displayDurationFromSecond,
} from '../../shared/date/date-factory';
import view from './formScore.html';

const name = 'formScore';

function ElementController() {
  const ctrl = this;
  ctrl.form = {
    displayDuration,
    displayDurationFromMillisecond,
    displayDurationFromSecond,
    dateString,
  };
  return ctrl;
}

app.component(name, {
  template: view,
  controller: [ElementController],
  bindings: {
    score: '<',
  },
});

export default name;
