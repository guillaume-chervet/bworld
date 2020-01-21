import React from 'react';
import PropTypes from 'prop-types';
import app from '../../app.module';

import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import Loadable from 'react-loadable';

export const LoadableVideo = Loadable({
  loader: () => import('./Player'),
  loading() {
    return <div>Loading...</div>;
  },
});

const name = 'mwVideo';
app.component(name, react2angular(LoadableVideo, ['file']));

export default name;
