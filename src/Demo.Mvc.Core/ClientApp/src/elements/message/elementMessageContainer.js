import React from 'react';
import { message as messageService } from '../../contact/message/message-factory';
import { master } from '../../shared/providers/master-provider';
import redux from '../../redux';

import './message.css';
import { MessageContainer } from './elementMessage-component';

export const MessageDirtyContainer = ({ element }) => {
  const moduleId = master.getModuleId();
  const user = redux.getState().user.user;
  const siteId = master.site.siteId;
  return (
    <MessageContainer
      element={element}
      siteId={siteId}
      moduleId={moduleId}
      user={user}
      sendMessageAsync={messageService.sendMessageAsync}></MessageContainer>
  );
};
