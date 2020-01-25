import app from '../../app.module';
import { menu } from './menu-factory';
import { connect } from 'react-redux';
import { react2angular } from 'react2angular';
import { withStore } from '../../reducers.config';
import React from 'react';

const getMenuItems = function(master) {
  const newMenu = menu.mapPublishedMenu(master.menu);
  const menuItems = [];
  const menuItemsTemp = menu.getMenuItems(newMenu.bottomMenuItems, false);
  if (menuItemsTemp) {
    for (let i = 0; i < menuItemsTemp.length; i++) {
      menuItems.push(menuItemsTemp[i]);
    }
  }
  return menuItems;
};

const name = 'menuBottom';

const MenuBottom = ({ menuItems, titleSite, currentPath }) => {
  const version = window.params.version;
  return (
    <footer>
      <div className="container">
        <div className="row">
          <div className="col-lg-12 ">
            <span>
              &copy; <b>{titleSite}</b> {new Date().getFullYear()}
            </span>
          </div>
          <div className="col-lg-12">
            <ul className="mw-footer-links">
              <li>version v{version}</li>
              {menuItems.map(menuItem => (
                <li key={menuItem.routePath}>
                  {' '}
                  <a href={menuItem.routePath}>{menuItem.title}</a>{' '}
                </li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </footer>
  );
};

const mapStateToProps = (state, ownProps) => {
  const master = state.master;
  return {
    menuItems: getMenuItems(master),
    titleSite: master.masterData.titleSite,
  };
};

const mapDispatchToProps = (dispatch, ownProps) => {
  return {};
};

const MenuBottomWithState = withStore(
  connect(
    mapStateToProps,
    mapDispatchToProps
  )(MenuBottom)
);

app.component(name, react2angular(MenuBottomWithState, ['currentPath']));

export default name;
