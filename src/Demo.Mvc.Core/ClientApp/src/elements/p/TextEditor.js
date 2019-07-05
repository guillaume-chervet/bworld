import React from 'react';
import PropTypes from 'prop-types';
import app from '../../app.module';

import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import Loadable from 'react-loadable';

const LoadableRteEditor = Loadable({
  loader: () => import('./RteEditor'),
  loading() {
    return <div>Loading...</div>;
  },
});

const name = 'textEditor';
app.component(name, react2angular(LoadableRteEditor, ['value', 'onChange']));

export default name;
