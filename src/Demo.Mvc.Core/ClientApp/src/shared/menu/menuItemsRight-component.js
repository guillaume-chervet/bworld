import app from '../../app.module';
import { menu } from './menu-factory';

import SocialMenuItem from '../../social/socialMenuItemRight-component';
import FreeMenuItem from '../../free/freeMenuItemRight-component';

import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import SubMenuItemsRight from './subMenuItemsRight-component';

const name = 'menuItemsRight';

app.component(
  name,
  react2angular(SubMenuItemsRight, [
    'start',
    'end',
    'menuItems',
    'filter',
    'isVisible',
    'currentPath',
  ])
);
