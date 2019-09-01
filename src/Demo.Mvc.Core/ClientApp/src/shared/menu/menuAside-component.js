import app from '../../app.module';
import history from '../../history';
import { userNotification } from '../../user/info/userNotification-factory';
import { user as userFactory } from '../../user/info/user-factory';
import { menu as menuFactory } from './menu-factory';
import { master as masterFactory } from '../providers/master-provider';
import MenuItemsRight from './menuItemsRight-component';
import { connect } from 'react-redux'
import {react2angular} from "react2angular";
import {withStore} from "../../reducers.config";
import React from "react";
import './menuRight.css';


const name = 'menuAside';


const MenuAside = ({user, menu, isCollapsed, currentPath}) => {

  const notification = userNotification.data;
  const isAdmin = menuFactory.isAdmin;
  const isUser = menuFactory.isUser;
  const isPrivate = menuFactory.isPrivate;
  const getInternalPath = masterFactory.getInternalPath;
  const isActive = menuFactory.isActive;

  const  logOff = function(e) {
    e.preventDefault();
    userFactory.logOffAsync().then(function() {
      history.path('/utilisateur/connexion');
    });
  };
  const toggleMenu = function() {
    menuFactory.updateMenu(!isCollapsed);
  };
  
  return (<div>
    {isAdmin() && (<div id="mw-admin-layer" className="mw-menu-layer" >
      <div><span>Administration</span></div>
    </div>)}
    {isUser() && (<div id="mw-user-layer" className="mw-menu-layer" >
      <div><span>Utilisateur</span></div>
    </div>)}
    {isPrivate() && <div id="mw-private-layer" className="mw-menu-layer">
      <div><span>Privée</span></div>
    </div>}
    {!isCollapsed && (<div id="menu-layer" onClick={toggleMenu}></div>)}
    {!isCollapsed && (<div className="aside" tabIndex="-1" role="dialog">
      <div className="aside-dialog">
        <div className="aside-content">
          <div className="aside-header" onClick={toggleMenu}>
            <button type="button" className="close">×</button>
            <h4 className="aside-title"><span className="glyphicon fa fa-bars" aria-hidden="true"></span> Menu</h4>
          </div>
          <div className="aside-body">
            <ul className="nav navbar-nav">
              {user.isAuthenticate && (<li className={isActive('/utilisateur/connexion') ? 'active' : ''}
                  className="visible-xs-block">
                <a href={getInternalPath('/utilisateur/connexion')}><span
                    className="glyphicon glyphicon-globe" aria-hidden="true"></span> Se connecter</a>
              </li>)}
              {user.isAuthenticate && (<li  className={isActive('/utilisateur/connexion') ? 'active' : ''}
                  className="visible-xs-block">
                <a href={getInternalPath('/utilisateur')}><span className="glyphicon glyphicon-user"
                                                                             aria-hidden="true"></span><span> {user.userName}</span>{notification.isUnreadMessage() && (<span className="badge"
                                                               
                                                               >{notification.numberUnreadMessage}</span>)}</a>
              </li>)}
              <li className="divider visible-xs-block"></li>
              <MenuItemsRight currentPath={currentPath} isVisible={!isPrivate()} start={0} end={100}
                                menuItems={menu.menuItems} />
              <MenuItemsRight currentPath={currentPath} isVisible={isPrivate()} start={0} end={100}
                                menuItems={menu.privateMenuItems} />
              { user.isAdministrator && (<> <li className="divider"></li>
              <li className={isActive('/administration') ? 'active' : ''}>
                <a href={getInternalPath('/administration')}><span className="glyphicon glyphicon-cog"
                                                                                aria-hidden="true"></span> Administration du site</a>
                <ul className="mw-submenu">
                  <li><a href={getInternalPath('/administration/statistiques')}><span
                      className="glyphicon glyphicon-stats" aria-hidden="true"></span> Statistiques</a></li>
                  <li><a href={getInternalPath('/administration/messages')}><span className="glyphicon glyphicon-envelope" aria-hidden="true"></span> Messages {notification.isSiteUnreadMessage() && (<span className="badge" >{notification.numberSiteUnreadMessage}</span>)}</a></li>
                </ul>
              </li></>)}
              {user.isSuperAdministrator && (<li
                  className={isActive('/super-administration') ? 'active' : ''}>
                <a href={getInternalPath('/super-administration')}><span className="glyphicon glyphicon-cog" aria-hidden="true"></span> Super administration</a>
              </li>)}
              
              {user.isAuthenticate && (<>
              <li className="divider"></li>
              <li >
                <a href="" onClick={logOff}><span className="glyphicon glyphicon-log-out"
                                                           aria-hidden="true"></span> Se déconnecter</a>
              </li></>)}
            </ul>
          </div>
        </div>
      </div>
    </div>)}
  </div>);
};



const mapStateToProps = (state, ownProps) => {
  return {
    user: state.user.user,
    menu: menuFactory.mapPublishedMenu(state.master.menu),
    isCollapsed: state.master.menuData.isCollapsed,
  };
};

const mapDispatchToProps = (dispatch, ownProps) => {
  return {};
};

const MenuAsideWithState = withStore(connect(
    mapStateToProps,
    mapDispatchToProps
)(MenuAside));

app.component(name, react2angular(MenuAsideWithState, ['currentPath']));

export default name;