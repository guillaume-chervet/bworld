import app from '../../app.module';

import React from 'react';
import { react2angular } from 'react2angular';

export const H1 = ({ element }) => {
  return <h1>{element.data}</h1>;
};

const name = 'elementH1';

app.component(name, react2angular(H1, ['element']));

export default name;
