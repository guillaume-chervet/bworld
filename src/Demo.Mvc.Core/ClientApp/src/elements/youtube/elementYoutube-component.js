import app from '../../app.module';
import React from 'react';
import { react2angular } from 'react2angular';
import Loadable from 'react-loadable';

import './youtube.css';

const LoadableYoutube = Loadable({
  loader: () => import('react-youtube'),
  loading() {
    return <div>Loading...</div>;
  },
});

export const YouTubeComponent = ({ element }) => {
  const opts = {};
  const onReady = e => console.log(e);
  return (
    <div className="mw-youtube">
      <LoadableYoutube
        videoId={element.data.url}
        opts={opts}
        onReady={onReady}
      />
    </div>
  );
};
