import { menu } from './menu-factory';

import SocialMenuItem from '../../social/socialMenuItem-component';
import FreeMenuItem from '../../news/newsMenuItem-component';

import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import classNames from 'classnames';
import { compose, withState, withHandlers } from 'recompose';

const enhance = compose(
  withState('isOpen', 'setOpen', false),
  withHandlers({
    onMouseEnter: ({ setOpen }) => () => setOpen(n => true),
    onMouseLeave: ({ setOpen }) => () => setOpen(n => false),
  })
);

const ChildItem = props => {
  const {
    menuItem,
    index,
    currentPath,
    onMouseLeave,
    onMouseEnter,
    isOpen,
  } = props;
  let Item = null;
  const isActive = menu.isActive(menuItem.routePath, currentPath);
  switch (menuItem.module) {
    case 'Social':
      Item = <SocialMenuItem menuItem={menuItem} currentPath={currentPath} />;
      break;
    default:
      Item = <FreeMenuItem menuItem={menuItem} currentPath={currentPath} />;
      break;
  }
  const componentClassName = classNames('mw-menu-item-' + index, {
    active: isActive,
    open: isOpen,
  });
  return (
    <li
      className={componentClassName}
      onMouseEnter={onMouseEnter}
      onMouseLeave={onMouseLeave}>
      {Item}
    </li>
  );
};

const EnhancedChildItem = enhance(ChildItem);

export default class SubMenuItems extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    const { menuItems, currentPath } = this.props;
    const listItems = menuItems.map((child, index) => (
      <EnhancedChildItem
        menuItem={child}
        index={index}
        key={child.moduleId}
        currentPath={currentPath}
      />
    ));
    return <React.Fragment>{listItems}</React.Fragment>;
  }
}
