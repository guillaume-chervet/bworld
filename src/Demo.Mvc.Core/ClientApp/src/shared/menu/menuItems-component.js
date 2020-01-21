import { menu } from './menu-factory';

import React from 'react';
import { SubMenuItems } from './subMenuItems-component';
import classNames from 'classnames';

const MenuItems = ({
  menuItems,
  end,
  start,
  filter,
  isVisible,
  currentPath,
  className,
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
        <SubMenuItems menuItems={childs} currentPath={currentPath} />
      </ul>
    </nav>
  );
};

export default MenuItems;
