import app from '../../app.module';
import React from 'react';
import { react2angular } from 'react2angular';
import { User } from '../user/mwUser-component';

const name = 'mwBottom';

export const Bottom = ({ data }) => {
  if(!data.isDisplayAuthor)
  {
    return null;
  }
  return (
      <div className="row">
        <div className="col-md-12 col-xs-12">
          <User data={data} />
        </div>
      </div>
  );
};

app.component(name, react2angular(Bottom, ['data']));
      