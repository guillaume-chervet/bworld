import app from '../../app.module';

import React from 'react';
import { react2angular } from 'react2angular';
import LoadableCodeAce from './LoadableCodeAce';

const modes = [
  'xml',
  'javascript',
  'json',
  'java',
  'python',
  'go',
  'csharp',
  'gerkin',
  'html',
  'css',
  'markdown',
  'less',
  'sass',
  'yaml',
  'typescript',
  'text',
  'powershell',
  'sql',
];

export default class CodePanelAdmin extends React.Component {
  constructor(props) {
    super(props);
    this.onFileNameChange.bind(this);
    this.onModeChange.bind(this);
    this.onChangeWrapper.bind(this);
  }
  onFileNameChange(event, data) {
    const fileName = event.target.value;
    const { element, onChange, file } = data;
    const newElement = {
      property: element.property,
      data: {
        files: [{ ...file, fileName }],
      },
    };
    onChange(newElement);
  }
  onModeChange(event, data) {
    const mode = event.target.value;
    const { element, onChange, file } = data;
    const newElement = {
      property: element.property,
      data: {
        files: [{ ...file, mode }],
      },
    };
    onChange(newElement);
  }
  onChangeWrapper(event, data) {
    const { element, onChange } = data;
    const newElement = {
      property: element.property,
      data: {
        files: [event.file],
      },
    };
    onChange(newElement);
  }
  render() {
    const { file, element, onChange } = this.props;

    return (
      <div key={file.id}>
        <div name="fieldForm" className="form-group">
          <label
            htmlFor="uFileName"
            className="col-sm-3 col-md-3 col-xs-12 control-label">
            Nom du fichier
          </label>
          <div className="col-sm-9 col-md-9 col-xs-12">
            <input
              type="text"
              id="uFileName"
              name="uFileName"
              className="form-control"
              value={file.fileName}
              onChange={e => this.onFileNameChange(e, this.props)}
            />
          </div>
        </div>
        <div name="fieldForm" className="form-group">
          <label
            htmlFor="uMode"
            className="col-sm-3 col-md-3 col-xs-12 control-label">
            Language
          </label>
          <div className="col-sm-9 col-md-9 col-xs-12">
            <select
              id="uMode"
              name="uMode"
              className="form-control"
              value={file.mode}
              onChange={e => this.onModeChange(e, this.props)}>
              <option value="">- Sélectionner -</option>
              {modes.map(m => (
                <option value={m}>{m}</option>
              ))}
            </select>
          </div>
        </div>
        <div name="fieldForm" className="form-group">
          <label
            htmlFor="uCodeName"
            className="col-sm-3 col-md-3 col-xs-12 control-label">
            Code
          </label>
          <div className="col-sm-9 col-md-9 col-xs-12">
            <LoadableCodeAce
              file={file}
              onChange={e => this.onChangeWrapper(e, { element, onChange })}
            />
          </div>
        </div>
      </div>
    );
  }
}
const name = 'elementCodePanel';
app.component(
  name,
  react2angular(CodePanelAdmin, ['file', 'element', 'onChange'])
);
