import app from '../../app.module';

import React from 'react';
import { react2angular } from 'react2angular';
import SubMenuItemsRight from './subMenuItemsRight-component';


const name = 'menuItemsRight';

const MenuItemsRight = SubMenuItemsRight;

export default MenuItemsRight;

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
