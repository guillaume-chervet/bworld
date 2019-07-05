import app from './app.module';
import loader from './shared/loader/loader-reducer';
import free from './free/free-reducer';
import user from './user/user-reducer';
import master from './shared/providers/master-reducer';
import thunk from 'redux-thunk';
import { combineReducers } from 'redux';

app.config([
  '$ngReduxProvider',
  $ngReduxProvider => {
    const rootReducer = combineReducers({
      loader,
      free,
      user,
      master,
    });
    $ngReduxProvider.createStoreWith(rootReducer, [thunk]);
  },
]);
