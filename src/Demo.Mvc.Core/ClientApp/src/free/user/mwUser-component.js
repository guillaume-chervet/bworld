import app from '../../app.module';
import {dateString} from '../../shared/date/date-factory';

import React from 'react';
import { react2angular } from 'react2angular';

const name = 'mwUser';

export const User = ({ data }) => {
  return (
        <p className="mw-author">
          <a rel="me" href={data.userInfo.authorUrl ? data.userInfo.authorUrl : ''}>
            <span itemProp="author">{data.userInfo.userName ? data.userInfo.userName : 'unknow'}</span>
          </a> <span>{dateString(data.createDate.getTime())}</span>
            {data.lastUpdateUserInfo.userName ? (<span>, mis à jour {data.userInfo.userName !== data.lastUpdateUserInfo.userName ? (<>par <a rel="me" href={data.lastUpdateUserInfo.authorUrl ? data.lastUpdateUserInfo.authorUrl:  ''}>
                    <span itemProp="author">{data.lastUpdateUserInfo.userName ? data.lastUpdateUserInfo.userName :  'unknow'}</span>
                </a></>) : null} {dateString(data.updateDate.getTime())}</span>
                ): null}
        </p>
  );
};

app.component(name, react2angular(User, ['data']));
      
export default name;