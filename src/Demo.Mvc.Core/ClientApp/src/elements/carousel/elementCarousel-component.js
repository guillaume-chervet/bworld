import app from '../../app.module';
import React from 'react';
import { react2angular } from 'react2angular';
import LoadableCarousel from './LoadableCarousel';
import './carousel.css';

export const CarouselComponent = ({element}) => {
  return (
      <div className="row mw-carousel">
      <LoadableCarousel infiniteLoop={true} showThumbs={false} autoPlay={true} >
        {element.data.map(slide =>(<div>
          <img src={slide.url} />
              {slide.description && (<p className="legend">{slide.description}</p>)}
        </div>)) }
      </LoadableCarousel>
      </div>
  );
};

const name = 'elementCarousel';
app.component(name, react2angular(CarouselComponent, ['element']));
