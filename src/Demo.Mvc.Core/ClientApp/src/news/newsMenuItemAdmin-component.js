import app from '../app.module';
import { isDraft, isDeleted } from '../shared/itemStates';
import { getIcon } from '../shared/icons';
import React from 'react';
import { react2angular } from 'react2angular';


const NewsMenuItemAdmin = ({ menuItem }) => {
  return (
      <React.Fragment>
        {' '}
        <a href={menuItem.routePath}>
          <span className={getIcon(menuItem)} />
          <span> {menuItem.title}</span>
        </a>
        {isDraft(menuItem) && (
            <span className="label label-default">Brouillon</span>
        )}
        {isDeleted(menuItem) && (
            <span className="label label-danger">Suprimé</span>
        )}
      </React.Fragment>
  );
};

export default NewsMenuItemAdmin;

const name = 'newsMenuItemAdmin';
app.component(name, react2angular(NewsMenuItemAdmin, ['menuItem']));
