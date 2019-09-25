import app from '../app.module';
import { master as masterProvider } from '../shared/providers/master-provider';
import { getIcon } from '../shared/icons';
import { breadcrumb } from './breadcrumb-factory';
import './breadcrumb.css';
import React from 'react';
import { react2angular } from 'react2angular';

const name = 'breadcrumb';

const MenuItemLinkBreadcrumb = ({item}) => {
  return (<li className={{active: item.active}} >
     {!item.active && <a href={masterProvider.getInternalPath(item.url)}>
        <span className={getIcon(item)}></span>
       {` ${item.title}`}
      </a>}
    { item.active && <>
      <span className={getIcon(item)}> </span>
      <span> {item.title}</span>
    </>}
  </li>);
};

export const Breadcrumb = ({master}) => {
  const items = breadcrumb.getItemsClean(master.path, master, master.routeCurrentModuleId);
  const isVisible = () => breadcrumb.isVisibleClean(master, master.path);

  return (
      <div>
        { isVisible() && <ol className="mw-breadcrumb breadcrumb">
          {items.map( item => <MenuItemLinkBreadcrumb item={item}></MenuItemLinkBreadcrumb>)}
        </ol>}
      </div>
  );
};

app.component(name, react2angular(Breadcrumb, ['master']));

export default name;

