import app from './app.module';
//import { master } from './shared/providers/master-provider';
import './history';
import './http';
import './sce';
import redux from './redux';

app.run([
  '$rootScope',
  'dummyhistory',
  'dummyhttp',
  'dummyredux',
  'dummysce',
  'dummymodal',
  'dummyroute',
  'dummycookie',
  'dummyrootscope',
  function($rootScope) {
    //$rootScope.master = master.master;
    const state = redux.getState();
    $rootScope.isDisplayMenu = state.master.menuData.isDisplayMenu;
  },
]);
