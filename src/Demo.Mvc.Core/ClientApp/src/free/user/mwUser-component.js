import app from '../../app.module';

import React from 'react';
import { react2angular } from 'react2angular';

const name = 'mwUser';

export const User = ({ data }) => {
  return (
        <p className="mw-author">
          <a rel="me" href={data.userInfo.authorUrl ? data.userInfo.authorUrl : ''}>
            <span itemProp="author">{data.userInfo.userName ? data.userInfo.userName : 'unknow'}</span>
          </a> le <span>{data.createDate.toString('dd/MM/yyyy à HH:mm')}</span>
            {data.lastUpdateUserInfo.userName ? (<span>, mis à jour par <a rel="me" href={data.lastUpdateUserInfo.authorUrl ? data.lastUpdateUserInfo.authorUrl : ''}>
                    <span itemProp="author">{data.lastUpdateUserInfo.userName ? data.lastUpdateUserInfo.userName :  'unknow'}</span>
                </a> le {data.updateDate.toString('dd/MM/yyyy à HH:mm')}</span>
                ): null}
        </p>
  );
};

app.component(name, react2angular(User, ['data']));
      
export default name;