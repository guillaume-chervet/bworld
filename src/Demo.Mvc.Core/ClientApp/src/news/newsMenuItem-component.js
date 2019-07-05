import { getIcon } from '../shared/icons';

import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import { menu } from '../shared/menu/menu-factory';
import { SubMenu } from '../free/freeMenuItemRight-component';
/*
const Ul = ({childs}) =>{

	<ul className="dropdown-menu">
		<li className={menu.isActive(menuItem.routePath) ? 'active' : ''}>
			<a href={menuItem.routePath}>
				<span className={getIcon(menuItem)}></span>
				<span>{menuItem.title}</span>
			</a>
		</li>
		<li className="divider"></li>
	</ul>;    
};*/

export default class NewsMenuItem extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    const { menuItem } = this.props;

    const childs = menu.getSecondMenuItems(menuItem.childs, 100, 0, null);
    if (childs && childs.length > 0) {
      return (
        <React.Fragment>
          <a href={menuItem.routePath}>
            <span className={getIcon(menuItem)} />
            <span> {menuItem.title}</span>
            <b className="caret" />
          </a>
          <SubMenu menuItem={menuItem} className="dropdown-menu" />
        </React.Fragment>
      );
    }

    return (
      <a href={menuItem.routePath}>
        <span className={getIcon(menuItem)} />
        <span> {menuItem.title}</span>
      </a>
    );
  }
}
