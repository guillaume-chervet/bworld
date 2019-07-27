import app from '../../app.module';
import React, { Fragment } from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import LoadableCodeAce from './LoadableCodeAce';

import './code.css';

const name = 'elementCode';

export const Code =({element}) => {
    return (
      <Fragment>
        <div className="col-sm-12 col-md-12 col-xs-12 mw-code-panel">
          {element.data.files.map(file => (
            <Fragment key={file.fileName}>
              <h3>{file.fileName}</h3>
              <LoadableCodeAce file={file} mw-readonly="'true'" />
            </Fragment>
          ))}
          ))}
        </div>
        <div className="clearfix" />
      </Fragment>
    );
};

app.component(name, react2angular(Code, ['element']));
