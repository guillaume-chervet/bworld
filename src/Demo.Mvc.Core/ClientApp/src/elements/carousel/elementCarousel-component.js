import app from '../../app.module';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import 'react-responsive-carousel/lib/styles/carousel.min.css';
import { Carousel } from 'react-responsive-carousel';

import './carousel.css';

export const CarouselComponent = ({element}) => {
  return (
      <div className="row mw-carousel">
      <Carousel>
        {element.data.map(slide =>(<div>
          <img src={slide.thumbnailUrl} />
              {slide.description && (<p className="legend">Legend 1</p>)}
        </div>)) }
      </Carousel>
      </div>
  );
};

const name = 'elementCarousel';
app.component(name, react2angular(CarouselComponent, ['element']));

export default name;

