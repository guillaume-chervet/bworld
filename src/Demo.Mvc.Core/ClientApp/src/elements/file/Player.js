import React from 'react';
import { Player } from 'video-react';

import 'video-react/dist/video-react.css';

class ResponsivePlayer extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    const { file } = this.props;
    return (
      <div className="player-wrapper">
        <Player>
          <source src={file.config.sources[0].src} />
        </Player>
      </div>
    );
  }
}

export default ResponsivePlayer;
