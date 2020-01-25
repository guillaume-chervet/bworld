import React from 'react';
import PropTypes from 'prop-types';
import app from '../../app.module';

import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import Loadable from 'react-loadable';

const LoadableMap = Loadable({
  loader: () => import('./Map'),
  loading() {
    return <div>Loading...</div>;
  },
});

const name = 'mwLoadableMap';
app.component(name, react2angular(LoadableMap, ['element']));

export default LoadableMap;
