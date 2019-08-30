import app from '../../app.module';
import { menu } from './menu-factory';

import React from 'react';
import { react2angular } from 'react2angular';
import SubMenuItem from './subMenuItems-component';
import classNames  from 'classnames';

const MenuItems = ( {
    menuItems,
    end,
    start,
    filter,
    isVisible,
    currentPath,
    className
}) => {
        if (!isVisible) {
            return null;
        }
        const currentMenuItems = [];
        const childs = menu.getSecondMenuItems(
            menuItems,
            end,
            start,
            filter,
            currentMenuItems
        );
        
        const newClassName = classNames('navbar-collapse', className);
        
        return (
            <nav className={newClassName}>
                <ul className="nav navbar-nav">
                    <SubMenuItem menuItems={childs} currentPath={currentPath} />
                </ul>
            </nav>
        );
};

export default MenuItems;

const name = 'menuItems';
app.component(
  name,
  react2angular(MenuItems, [
    'start',
    'end',
    'menuItems',
    'filter',
    'isVisible',
    'currentPath',
  ])
);
