import app from '../../app.module';
import { userNotification } from '../../user/info/userNotification-factory';
import { menu as menuFactory } from './menu-factory';
import { master } from '../providers/master-provider';
import { withStore } from '../../reducers.config';
import React from 'react';
import { connect } from 'react-redux';
import { react2angular } from 'react2angular';
import MenuItems from './menuItems-component';

import './menu.css';

const name = 'mainMenu';

const MainMenu = ({ user, menu, isDisplayMenu, isCollapsed, currentPath }) => {
  const notification = userNotification.data;
  const updateMenu = e => {
    e.preventDefault();
    menuFactory.updateMenu(!isCollapsed);
  };
  const getInternalPath = master.getInternalPath;
  const isActive = menuFactory.isActive;
  const isPrivate = menuFactory.isPrivate;
  const getUserName = function(initial) {
    const _user = user;
    if (initial) {
      return (
        _user.firstName.slice(0, 1).toUpperCase() +
        _user.lastName.slice(0, 1).toUpperCase()
      );
    }
    return _user.firstName;
  };

  return (
    <div>
      {isDisplayMenu && (
        <ul className="mw-navbar-user">
          {!user.isAuthenticate && (
            <li className={isActive('/utilisateur/connexion') ? 'active' : ''}>
              <a
                href={getInternalPath('/utilisateur/connexion')}
                className="btn btn-default btn-lg">
                <span
                  className="glyphicon glyphicon-globe"
                  role="button"></span>{' '}
                Se connecter
              </a>
            </li>
          )}
          {user.isAuthenticate && (
            <li className={isActive('/utilisateur') ? 'active' : ''}>
              <a
                href={getInternalPath('/utilisateur')}
                className="btn btn-default btn-lg">
                <span className="fa fa-user" role="button"></span>{' '}
                <span className="hidden-xs">{getUserName()}</span>
                {notification.isUnreadMessage() && (
                  <span className="badge">
                    {notification.numberUnreadMessage}
                  </span>
                )}
              </a>
            </li>
          )}
          <li>
            <a href="#" onClick={updateMenu} className="btn btn-default btn-lg">
              <span className="glyphicon fa fa-bars" aria-hidden="true"></span>{' '}
              <span className="hidden-xs">Menu</span>
            </a>
          </li>
        </ul>
      )}
      <header className="navbar navbar-default" id="top" role="banner">
        <div className="container mw-navbar-center">
          <MenuItems
            isVisible={isDisplayMenu && !isPrivate()}
            currentPath={currentPath}
            menuItems={menu.menuItems}
            start={0}
            end={100}
            className="hidden-xs"
          />
          <MenuItems
            isVisible={isDisplayMenu && !isPrivate()}
            currentPath={currentPath}
            menuItems={menu.menuItems}
            start={0}
            end={100}
            filter={'Social'}
            className="visible-xs"
          />
          <MenuItems
            isVisible={isDisplayMenu && isPrivate()}
            currentPath={currentPath}
            menuItems={menu.privateMenuItems}
            start={0}
            end={100}
            className="hidden-xs"
          />
        </div>
      </header>
    </div>
  );
};

const mapStateToProps = (state, ownProps) => {
  return {
    user: state.user.user,
    menu: menuFactory.mapPublishedMenu(state.master.menu),
    isDisplayMenu: state.master.menuData.isDisplayMenu,
    isCollapsed: state.master.menuData.isCollapsed,
  };
};

const mapDispatchToProps = (dispatch, ownProps) => {
  return {};
};

const MainMenuWithState = withStore(
  connect(
    mapStateToProps,
    mapDispatchToProps
  )(MainMenu)
);

app.component(name, react2angular(MainMenuWithState, ['currentPath']));

export default name;
