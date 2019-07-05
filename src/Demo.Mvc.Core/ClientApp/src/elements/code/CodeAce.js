import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import brace from 'brace';
import AceEditor from 'react-ace';

import 'brace/mode/java';
import 'brace/mode/csharp';
import 'brace/mode/javascript';
import 'brace/mode/python';
import 'brace/mode/xml';
import 'brace/mode/ruby';
import 'brace/mode/sass';
import 'brace/mode/markdown';
import 'brace/mode/json';
import 'brace/mode/html';
import 'brace/mode/handlebars';
import 'brace/mode/golang';
import 'brace/mode/coffee';
import 'brace/mode/css';

import 'brace/theme/github';

export class CodeAce extends React.Component {
  constructor(props) {
    super(props);
    this.onChange.bind(this);
  }
  onChange(e) {
    const { file, onChange } = this.props;
    if (onChange) {
      onChange({
        file: {
          ...file,
          code: e,
        },
      });
    }
  }
  render() {
    const { file } = this.props;
    return (
      <AceEditor
        mode={file.mode}
        theme="github"
        onChange={e => this.onChange(e)}
        value={file.code}
        name={file.id}
        width=""
        editorProps={{ $blockScrolling: true }}
      />
    );
  }
}

export default CodeAce;
