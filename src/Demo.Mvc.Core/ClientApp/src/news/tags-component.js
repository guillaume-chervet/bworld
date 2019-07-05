import app from '../app.module';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';

function hashCode(str) {
  // java String#hashCode
  let hash = 0;
  for (var i = 0; i < str.length; i++) {
    hash = str.charCodeAt(i) + ((hash << 5) - hash);
  }
  return hash;
}

function intToRGB(i) {
  const c = (i & 0x00ffffff).toString(16).toUpperCase();

  return '00000'.substring(0, 6 - c.length) + c;
}

function stringToRGB(str) {
  return intToRGB(hashCode(str));
}

const name = 'tags';

class Tags extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    return this.props.tags.map(tag => (
      <span
        className="label"
        key={tag.id}
        style={{
          backgroundColor: `#${stringToRGB(tag.id)}`,
          marginRight: '2px',
          marginLeft: '2px',
        }}>
        {tag.name}{' '}
      </span>
    ));
  }
}

app.component(name, react2angular(Tags, ['tags']));

export default name;
