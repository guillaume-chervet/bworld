import React from 'react';
import Loadable from 'react-loadable';

const LoadableCarousel = Loadable({
  loader: () => import('./Carousel'),
  loading() {
    return <div>Loading...</div>;
  },
});

export default LoadableCarousel;
