import React from 'react';
import app from '../../app.module';

import { react2angular } from 'react2angular';
import Loadable from 'react-loadable';

export const LoadableRteEditor = Loadable({
  loader: () => import('./RteEditor'),
  loading() {
    return <div>Loading...</div>;
  },
});

const name = 'textEditor';
app.component(name, react2angular(LoadableRteEditor, ['value', 'onChange']));

export default name;
