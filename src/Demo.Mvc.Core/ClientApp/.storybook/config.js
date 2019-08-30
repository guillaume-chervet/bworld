import { configure } from '@storybook/react';

function loadStories() {
    require('../src/index.stories.js');
    require('../src/elements/message/elementMessage.stories');
    // You can require as many stories as you need.
}

configure(loadStories, module);