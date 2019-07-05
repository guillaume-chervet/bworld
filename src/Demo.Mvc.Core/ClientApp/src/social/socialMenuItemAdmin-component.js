import app from '../app.module';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import NewsMenuItemAdmin from '../news/newsMenuItemAdmin-component';

const name = 'socialMenuItemAdmin';
app.component(name, react2angular(NewsMenuItemAdmin, ['menuItem']));
