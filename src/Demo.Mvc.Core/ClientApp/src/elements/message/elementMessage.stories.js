import React from 'react';
import { storiesOf } from '@storybook/react';
import { MessageContainer } from './elementMessage-component';

storiesOf('Elementmessage', module)
    .add('youhou', () => {
        const element = {data:{subjects:[]}};
        const user = {isAuthenticate:true};
        const moduleId= "";
        const sendMessageAsync = () => console.log("sendMessageAsync");
        return (  <MessageContainer element={element} user={user} moduleId={moduleId} sendMessageAsync={sendMessageAsync} />
    );});   