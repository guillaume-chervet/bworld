import { getIcon } from '../shared/icons';

import React from 'react';
import NewsMenuItem from '../news/newsMenuItem-component';
import SubMenuItemsRight from '../shared/menu/subMenuItemsRight-component';

export const SubMenu = ({ menuItems, className, currentPath }) => {
  if (!menuItems || menuItems.length <= 0) {
    return null;
  }

  return (
    <ul className={className}>
      <SubMenuItemsRight
        start={0}
        end={100}
        menuItems={menuItems}
        currentPath={currentPath}
      />
    </ul>
  );
};

SubMenu.defaultProps = {
  className: 'mw-submenu',
};

const FreeMenuItemRight = ({ menuItem, currentPath }) => {
  const childs = menuItem.childs;
  if (!childs || childs.length <= 0) {
    return <NewsMenuItem menuItem={menuItem} currentPath={currentPath} />;
  }

  return (
      <>
        <a href={menuItem.routePath}>
          <span className={getIcon(menuItem)} />
          <span> {menuItem.title}</span>
          <b className="caret" />
        </a>
        <SubMenu menuItem={menuItem.childs} currentPath={currentPath} />
      </>
  );
};

export default FreeMenuItemRight
