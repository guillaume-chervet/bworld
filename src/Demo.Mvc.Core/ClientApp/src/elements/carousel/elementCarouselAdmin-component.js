import app from '../../app.module';
import {ElementAdmin} from "../elementAdmin-component";
import {Upload} from "../file/upload-component";
import {react2angular} from "react2angular";
import React from "react";
import {ElementCarousel} from "./elementCarousel-component";

const name = 'elementCarouselAdmin';

export const ElementCarouselAdmin = ({ element, mode, onChange }) => {
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Carousel'}
          adminEdit={<Upload element={element} onChange={onChange} mode={mode}/>}
          adminView={<ElementCarousel element={element} />}>
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementCarouselAdmin, ['element', 'mode', 'onChange']));

export default name;

