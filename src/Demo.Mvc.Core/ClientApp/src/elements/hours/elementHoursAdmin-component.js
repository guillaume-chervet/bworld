import app from '../../app.module';
import view from './hours_admin.html';

const name = 'elementHoursAdmin';

class Controller {
  $onInit() {
    const ctrl = this;

    ctrl.ismeridian = false;
    ctrl.mstep = 15;
    ctrl.hstep = 1;

    ctrl.add = function(day) {
      var beginDate = new Date();
      beginDate.setHours(8);
      beginDate.setMinutes(0);

      var endDate = new Date();
      endDate.setHours(18);
      endDate.setMinutes(30);

      day.hours.push({
        begin: beginDate,
        end: endDate,
      });
    };

    ctrl.delete = function(day, hour) {
      var index = day.hours.indexOf(hour);
      if (index > -1) {
        day.hours.splice(index, 1);
      }
    };

    ctrl.isLastElement = function(day, hour) {
      var index = day.hours.indexOf(hour);
      if (index === day.hours.length - 1) {
        return true;
      }
      return false;
    };

    ctrl.getClassLabel = function(element) {
      if (element.$level <= 2) {
        return 'col-sm-2 col-md-2 col-xs-12';
      }
      return 'col-sm-12 col-md-12 col-xs-12';
    };

    ctrl.getClassField = function(element) {
      if (element.$level <= 2) {
        return 'col-sm-10 col-md-9 col-xs-12';
      }
      return 'col-sm-12 col-md-12 col-xs-12';
    };

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;
