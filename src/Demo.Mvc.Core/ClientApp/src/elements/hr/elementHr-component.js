import app from '../../app.module';

import React from 'react';
import { react2angular } from 'react2angular';

export const HR = ({ element }) => {
  const className = `mw-hr mw-hr${element.data.type}`;
  return <hr className={className} />;
};

const name = 'elementHr';
app.component(name, react2angular(HR, ['element']));

export default name;
