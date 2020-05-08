﻿import app from '../../app.module';
import { H1 } from './elementH1-component';
import { ElementAdmin } from '../elementAdmin-component';
import React from 'react';
import { react2angular } from 'react2angular';
import { ElementMenuH1 } from '../elementMenuH1-component';

const name = 'elementH1Admin';

const H1Edit = ({element, onChange}) => {
  return (<input className="form-control" type="text" name="field" id={element.property} value={element.data} onChange={onChange} />);
};

export const ElementPAdmin = ({ element, mode, onChange }) => {
  const onChangeWrapper = (e) => {
    onChange({ what: "element-edit", element: {...element, data: e.target.value}});
  };
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Titre principal'}
          adminEdit={<H1Edit element={element} onChange={onChangeWrapper} />}
          adminView={<H1 element={element} />}
          adminMenu={<ElementMenuH1 element={element} onChange={onChange} />}
      >
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementPAdmin, ['element', 'mode', 'onChange']));

export default name;