import app from '../../app.module';
import redux from '../../redux';
import { loaderAdd, loaderClear, loaderRemove } from './loader-actions';

const name = 'Loader';

let _loader = {
  isLoading: false,
  message: 'Chargement...',
};

function add(message) {
  const dispatch = redux.getDispatch();
  const data = loaderAdd(message);
  dispatch(data);
}

function remove() {
  const dispatch = redux.getDispatch();
  const data = loaderRemove();
  dispatch(data);
}

function clear() {
  const dispatch = redux.getDispatch();
  dispatch(loaderClear());
}

export const loader = {
  add,
  remove,
  clear,
  loader: _loader,
};

app.factory(name, [
  '$rootScope',
  function($rootScope) {
    $rootScope.$on('$routeChangeSuccess', function() {
      remove();
    });
    $rootScope.$on('$routeChangeStart', function() {
      add();
    });
    $rootScope.loader = _loader;
    return loader;
  },
]);

export default name;
