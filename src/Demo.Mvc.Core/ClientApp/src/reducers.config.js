import app from './app.module';
import loader from './shared/loader/loader-reducer';
import free from './free/free-reducer';
import user from './user/user-reducer';
import master from './shared/providers/master-reducer';
//import thunk from 'redux-thunk';
import { Provider } from 'react-redux'
import { combineReducers, createStore } from 'redux';
import React from "react";

let _store = null;

app.config([
  '$ngReduxProvider',
  $ngReduxProvider => {
    const rootReducer = combineReducers({
      loader,
      free,
      user,
      master,
    });

    _store = createStore(rootReducer, []);
    $ngReduxProvider.provideStore(_store);
  },
]);


export const withStore = (Component) => () => (<Provider store={_store}>
  <Component />
</Provider>);