import app from '../../app.module';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';

class Paragraphe extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    return (
      <div
        id={this.props.element.property}
        dangerouslySetInnerHTML={{ __html: this.props.element.data }}
      />
    );
  }
}

const name = 'elementP';
app.component(name, react2angular(Paragraphe, ['element']));

export default name;
