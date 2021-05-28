import app from '../../app.module';
import React from 'react';
import { react2angular } from 'react2angular';

export const Paragraphe = ({ element }) => {
  return (
    <div
      id={element.property}
      dangerouslySetInnerHTML={{ __html: element.data }}
    />
  );
};