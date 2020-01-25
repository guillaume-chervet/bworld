import app from '../app.module';
import NewsMenuItem from '../news/newsMenuItem-component';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';

const name = 'addSiteMenuItemRight';

export default NewsMenuItem;

app.component(name, react2angular(NewsMenuItem, ['menuItem']));
