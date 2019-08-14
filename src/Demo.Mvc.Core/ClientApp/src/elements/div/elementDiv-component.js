import app from '../../app.module';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import {Paragraphe} from '../p/elementP-component';
import {H1} from '../h1/elementH1-component';
import {Address} from '../address/elementAddress-component';
import {Code} from '../code/elementCode-component';
import {HR} from '../hr/elementHr-component';
import {DaysHours} from '../hours/elementHours-component';
import {Link} from '../link/elementLink-component';
import {GalleryFile} from '../file/elementFile-component';
import {Grid} from '../grid/elementGrid-component';
import { CarouselComponent } from '../carousel/elementCarousel-component';
import { YouTubeComponent } from '../youtube/elementYoutube-component';
import { MessageDirtyContainer } from '../message/elementMessageContainer';

const Switch = ({element}) => {
  switch (element.type) {
    case 'p':
      return (<Paragraphe element={element}/>);
        case 'h1':
        return (<H1 element={element}/>);
          case 'address':
          return (<Address element={element}/>);
            case 'code':
            return (<Code element={element}/>);
              case 'hr':
              return (<HR element={element}/>);
                case 'hours':
                return (<DaysHours element={element}/>);
                  case 'link':
                  return (<Link element={element}/>);
                    case 'file':
                    return (<GalleryFile element={element}/>);
                      case 'grid':
                      return (<Grid element={element} />);
                        case 'div':
                        return (<Switch element={element} />);
    case 'carousel':
      return (<CarouselComponent element={element} />);
    case 'message':
      return (<MessageDirtyContainer element={element} />);
    case 'youtube':
      return (<YouTubeComponent element={element} />);
        default:
      return <div>default {element.type}</div>;
  }
};

export const Div = ({element}) => {
  return (
      <div>
        {element.childs.map(child => <Switch element={child} /> )}
      </div>
  );
};

const name = 'elementDiv';
app.component(name, react2angular(Div, ['element']));

export default name;