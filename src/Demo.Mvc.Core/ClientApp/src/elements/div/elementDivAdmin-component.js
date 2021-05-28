import app from '../../app.module';
import {YouTubeComponent} from "../youtube/elementYoutube-component";
import {react2angular} from "react2angular";
import React from "react";
import {H1Admin} from "../h1/elementH1Admin-component";
import {PAdmin} from "../p/elementPAdmin-component";
import {ElementAddressAdmin} from "../address/elementAddressAdmin-component";
import {ElementCodeAdmin} from "../code/elementCodeAdmin-component";
import {ElementHRAdmin} from "../hr/elementHrAdmin-component";
import {ElementHoursAdmin} from "../hours/elementHoursAdmin-component";
import {ElementLinkAdmin} from "../link/elementLinkAdmin-component";
import {ElementFileAdmin} from "../file/elementFileAdmin-component";
import {ElementCarouselAdmin} from "../carousel/elementCarouselAdmin-component";
import {ElementMessageAdmin} from "../message/elementMessageAdmin-component";
import {ElementGridAdmin} from "../grid/elementGridAdmin-component";

const name = 'elementDivAdmin';

const Switch = ({ element, onChange }) => {
  switch (element.type) {
    case 'p':
      return <PAdmin element={element} onChange={onChange} />;
    case 'h1':
      return <H1Admin element={element} onChange={onChange} />;
    case 'address':
      return <ElementAddressAdmin element={element} onChange={onChange} />;
    case 'code':
      return <ElementCodeAdmin element={element} onChange={onChange} />;
    case 'hr':
      return <ElementHRAdmin element={element} onChange={onChange} />;
    case 'hours':
      return <ElementHoursAdmin element={element} onChange={onChange} />;
    case 'link':
      return <ElementLinkAdmin element={element} onChange={onChange} />;
    case 'file':
      return <ElementFileAdmin element={element} onChange={onChange} />;
    case 'grid':
      return <ElementGridAdmin element={element}  onChange={onChange} />;
    case 'div':
      return <Switch element={element} onChange={onChange} />;
    case 'carousel':
      return <ElementCarouselAdmin element={element} onChange={onChange} />;
    case 'message':
      return <ElementMessageAdmin element={element} onChange={onChange} />;
    case 'youtube':
      return <YouTubeComponent element={element} onChange={onChange} />;
    default:
      return <div>default {element.type}</div>;
  }
};

export const DivAdmin = ({ element, onChange }) => {
  return (
      <div>
        {element.childs.map(child => (
            <Switch key={child.property} element={child} onChange={onChange} />
        ))}
      </div>
  );
};

app.component(name, react2angular(DivAdmin, ['element', "onChange"]));

export default name;

