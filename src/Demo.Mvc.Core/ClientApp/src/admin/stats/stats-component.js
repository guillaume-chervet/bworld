import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { stats } from './stats-factory';
import { breadcrumb } from '../../breadcrumb/breadcrumb-factory';
import view from './stats.html';


import './stats.css';

const name = 'stats';

class Controller {
  $onInit() {
    const ctrl = this;
    const init = function() {
      page.setTitle('Statistiques', page.types.admin);
      const breadcrumbs = breadcrumb.getItems();
      breadcrumbs[breadcrumbs.length - 1].module = 'Stats';
    };

    init();
    ctrl.data = stats.data;
    ctrl.filter = stats.filter;
    ctrl.isNextDisabled = stats.isNextDisabled;
    ctrl.chartObject = {};
    const rows = [];

    const initRow = function() {
      if (stats.data && stats.data.hours && stats.data.hours.length > 0) {
        rows.length = 0;
        for (let i = 0; i < stats.data.hours.length; i++) {
          const hour = stats.data.hours[i];
          rows.push({
            c: [
              {
                v: hour.hour + 'h',
              },
              {
                v: hour.nbView,
              },
              {
                v: hour.nbNewClientSession,
              },
              {
                v: hour.nbNewCookieSession,
              },
            ],
          });
        }
      } else {
        for (let j = 0; j < rows.length; j++) {
          rows[j].c[1].v = 0;
        }
      }
    };

    initRow();
    ctrl.next = () => {
      stats.nextAsync().then(function() {
        init();
        initRow();
      });
    };
    ctrl.previous =() => {
      stats.previousAsync().then(function() {
        init();
        initRow();
      });
    };

    ctrl.chartObject.data = {
      cols: [
        {
          id: 't',
          label: 'Heure',
          type: 'string',
        },
        {
          id: 's',
          label: 'Nombre de vue',
          type: 'number',
        },
        {
          id: 'v',
          label: 'Nombre de visite',
          type: 'number',
        },
        {
          id: 'v',
          label: 'Nombre nouveaux visiteurs',
          type: 'number',
        },
      ],
      rows: rows,
    };

    // $routeParams.chartType == BarChart or PieChart or ColumnChart...;
    ctrl.chartObject.type = 'LineChart';
    ctrl.chartObject.options = {
      title: 'Nombre de vue par heure',
      legend: {
        position: 'bottom',
      },
    };

    ctrl.truncate = function(myString) {
      const length = 30;
      if (myString && myString.length > length) {
        const myTruncatedString = myString.substring(0, length);
        return myTruncatedString + ' ...';
      } else {
        return myString;
      }
    };

    ctrl.getClass = function(title) {
      if (!title) {
        return '';
      }
      if (title.indexOf('Utilisateur') === 0) {
        return 'mw-user-link';
      }
      if (title.indexOf('SuperAdministration') === 0) {
        return 'mw-superadmin-link';
      }
      if (title.indexOf('Administration') === 0) {
        return 'mw-admin-link';
      }
      return '';
    };

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {},
});

export default name;
