import React from 'react';
import PropTypes from 'prop-types';
import RichTextEditor from 'react-rte';

import './RteEditor.css';

class RteEditor extends React.Component {
  constructor(props) {
    super(props);

    var value = null;
    if (props.value) {
      value = RichTextEditor.createValueFromString(props.value, 'html');
    } else {
      value = RichTextEditor.createEmptyValue();
    }
    this.state = {
      value,
    };
    this.onChange.bind(this);
  }
  onChange(value) {
    this.setState({ value });
    if (this.props.onChange) {
      // Send the changes up to the parent component as an HTML string.
      // This is here to demonstrate using `.toString()` but in a real app it
      // would be better to avoid generating a string on each change.
      this.props.onChange(value.toString('html'));
    }
  }

  render() {
    return (
      <RichTextEditor
        className="mw-rte"
        toolbarClassName="mw-rte-toolbar"
        editorClassName="mw-rte-editor"
        value={this.state.value}
        onChange={v => this.onChange(v)}
      />
    );
  }
}

RteEditor.propTypes = {
  onChange: PropTypes.func,
};

export default RteEditor;
