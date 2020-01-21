const injector = window.angular.injector(['ng']);
const $q = injector.get('$q');

export default {
  reject: $q.reject,
  defer: $q.defer,
  all: $q.all,
};
