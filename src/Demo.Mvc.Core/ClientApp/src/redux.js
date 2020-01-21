import app from './app.module';

let _ngRedux = null;

app.factory('dummyredux', [
  '$ngRedux',
  function($ngRedux) {
    _ngRedux = $ngRedux;
    return {};
  },
]);

const redux = {
  getDispatch: () => {
    return _ngRedux.dispatch;
  },
  getConnect: () => {
    return _ngRedux.connect;
  },
  getState: () => {
    return _ngRedux.getState();
  },
};

export default redux;
