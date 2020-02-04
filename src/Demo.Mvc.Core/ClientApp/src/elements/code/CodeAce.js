import React from 'react';
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

export const CodeAce = ({ file, onChange }) => {
  const onChangeInternal = e => {
    if (onChange) {
      onChange({
        file: {
          ...file,
          code: e,
        },
      });
    }
  };
  return (
    <AceEditor
      mode={file.mode}
      theme="github"
      onChange={onChangeInternal}
      value={file.code}
      name={file.id}
      width=""
      editorProps={{ $blockScrolling: true }}
    />
  );
};

export default CodeAce;
