import app from '../app.module';
import NewsMenuItem from './newsMenuItem-component';
import React from 'react';
import { react2angular } from 'react2angular';

const name = 'newsMenuItemRight';

app.component(name, react2angular(NewsMenuItem, ['menuItem']));
