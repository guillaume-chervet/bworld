import app from '../../app.module';

import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';

class H1 extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    return <h1>{this.props.element.data}</h1>;
  }
}

const name = 'elementH1';

app.component(name, react2angular(H1, ['element']));

export default name;
