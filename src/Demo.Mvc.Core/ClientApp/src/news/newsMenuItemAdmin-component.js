import app from '../app.module';
import { isDraft, isDeleted } from '../shared/itemStates';
import { getIcon } from '../shared/icons';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';

export default class NewsMenuItemAdmin extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    const { menuItem } = this.props;
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
  }
}

const name = 'newsMenuItemAdmin';
app.component(name, react2angular(NewsMenuItemAdmin, ['menuItem']));
