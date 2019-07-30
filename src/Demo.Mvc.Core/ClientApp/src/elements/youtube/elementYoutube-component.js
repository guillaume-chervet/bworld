import app from '../../app.module';
import React from 'react';
import ReactDOM from 'react-dom';
import YouTube from 'react-youtube';
import { react2angular } from 'react2angular';

import './youtube.css';

export const YouTubeComponent = ({element}) => {
  const opts = {};
  return (
      <YouTube
          className="mw-youtube"
          videoId={element.data.url}
          opts={opts}
          onReady={this._onReady}
      />
  );
};

const name = 'elementYoutube';
app.component(name, react2angular(YouTubeComponent, ['element']));

export default name;

