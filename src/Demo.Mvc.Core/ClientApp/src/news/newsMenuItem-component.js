import { getIcon } from '../shared/icons';

import React from 'react';
import { menu } from '../shared/menu/menu-factory';
import { SubMenu } from '../free/freeMenuItemRight-component';

const NewsMenuItem = ({ menuItem }) => {
  const childs = menu.getSecondMenuItems(menuItem.childs, 100, 0, null);
  if (childs && childs.length > 0) {
    return (
      <React.Fragment>
        <a href={menuItem.routePath}>
          <span className={getIcon(menuItem)} />
          <span> {menuItem.title}</span>
          <b className="caret" />
        </a>
        <SubMenu menuItems={childs} className="dropdown-menu" />
      </React.Fragment>
    );
  }

  return (
    <a href={menuItem.routePath}>
      <span className={getIcon(menuItem)} />
      <span> {menuItem.title}</span>
    </a>
  );
};

export default NewsMenuItem;
