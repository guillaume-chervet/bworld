import app from '../../app.module';

import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';

class HR extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    const className = `mw-hr mw-hr${this.props.element.data.type}`;
    return <hr className={className} />;
  }
}

const name = 'elementHr';
app.component(name, react2angular(HR, ['element']));

export default name;
