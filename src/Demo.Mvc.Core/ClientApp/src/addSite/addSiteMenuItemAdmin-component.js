import app from '../app.module';
import NewsMenuItemAdmin from '../news/newsMenuItemAdmin-component';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';

const name = 'addSiteMenuItemAdmin';

app.component(name, react2angular(NewsMenuItemAdmin, ['menuItem']));
