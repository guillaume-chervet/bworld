import app from '../../app.module';

import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import Loadable from 'react-loadable';
const name = 'elementCodeAce';

const LoadableCodeAce = Loadable({
  loader: () => import('./CodeAce'),
  loading() {
    return <div>Loading...</div>;
  },
});

app.component(
  name,
  react2angular(LoadableCodeAce, ['file', 'mwReadonly', 'onChange'])
);

export default LoadableCodeAce;
