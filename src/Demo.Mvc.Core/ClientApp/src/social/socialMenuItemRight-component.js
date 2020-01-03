import app from '../app.module';
import React from 'react';
import { react2angular } from 'react2angular';
import SocialMenuItem from './socialMenuItem-component';

const name = 'socialMenuItemRight';
app.component(name, react2angular(SocialMenuItem, ['menuItem']));

export default SocialMenuItem;
