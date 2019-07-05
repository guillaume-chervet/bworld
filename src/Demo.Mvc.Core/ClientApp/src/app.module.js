import 'core-js/fn/array/find';

import angular from 'angular';
import angularAnimate from 'angular-animate';
import angularRoute from 'angular-route';
import angularSanitize from 'angular-sanitize';
import angularCookies from 'angular-cookies';
import angularTouch from 'angular-touch';
import angularGoogleChart from 'angular-google-chart';
import angularBoostrap from 'angular-ui-bootstrap';
import angularSocialLink from 'angular-social-links';
import angularYoutubeMb from 'ng-youtube-embed';
import angularMultiSelect from '../node_modules/isteven-angular-multiselect/isteven-multi-select';
import ngFileUpload from 'ng-file-upload';
import colorPicker from 'angular-bootstrap-colorpicker';
import pagination from 'angular-utils-pagination';
import validation from 'mw.validation';
import validationAngular from 'mw.angular.validation';
import humanizeDuration from 'humanize-duration';
import moment from 'moment';
import angularTimer from 'angular-timer';

import 'ng-redux';
window.moment = moment;
window.mw = validation;
window.humanizeDuration = humanizeDuration;
// Hack login facebook
if (window.location.href.indexOf('#_=_') > 0) {
  const href = window.location.href.replace('#_=_', '#');
  window.location = href;
}

const app = window.angular.module(
  'mw',
  [
    'ngRoute',
    'ngCookies',
    'ngTouch',
    'ngAnimate',
    'ngSanitize',
    'ngFileUpload',
    'ngRedux',
    'ui.bootstrap',
    'mw.validation',
    'colorpicker.module',
    'googlechart',
    'socialLinks',
    'angularUtils.directives.dirPagination',
    'timer',
    'ngYoutubeEmbed',
    'isteven-multi-select',
  ],
  function() {}
);

export default app;
