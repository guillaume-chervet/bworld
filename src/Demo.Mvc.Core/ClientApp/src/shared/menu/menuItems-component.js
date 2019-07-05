import app from '../../app.module';
import { menu } from './menu-factory';

import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import SubMenuItem from './subMenuItems-component';

export default class MenuItems extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    const {
      menuItems,
      end,
      start,
      filter,
      isVisible,
      currentPath,
    } = this.props;
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
    return (
      <nav className="navbar-collapse">
        <ul className="nav navbar-nav">
          <SubMenuItem menuItems={childs} currentPath={currentPath} />
        </ul>
      </nav>
    );
  }
}

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
