import app from '../../app.module';
import view from './hours_admin.html';
import {ElementAdmin} from "../elementAdmin-component";
import {YouTubeComponent} from "../youtube/elementYoutube-component";
import {react2angular} from "react2angular";
import React from "react";
import {DaysHours} from "./elementHours-component";
import {preventDefault} from "leaflet/src/dom/DomEvent";

const name = 'elementHoursAdmin';

class Controller {
  $onInit() {
   

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;

const YesNo =({yes, onChange}) =>{
  
  return <><p>
    <input type="radio" name="yes_no" checked={yes} onChange={()=>onChange(true)}>Ouvert</input>
  </p>
  <p>
    <input type="radio" name="yes_no" checked={!yes} onChange={()=>onChange(false)}>Fermé</input>
  </p></>
  
}

const HoursAdmin = ({ctrl, element, onAdd, onDelete, onOpen}) => {
  const level = element.$level.toString();
  
  switch (level) {
    case "3":
      return <>{element.data.days.map((day, indexDay) => <div className="form-group">
        <label className="col-xs-12 control-label">{day.label}</label>
        <div className=" col-xs-12">
          <div className="row">
            <div className="btn-group col-xs-12">
              <YesNo yes={day.isOpen} onChange={(isOpen) => onOpen(day, indexDay, isOpen)}/>
            </div>
            <div className="col-xs-12">
              {day.hours.map((hour, indexHour) =><>
              {day.isOpen && <div className="row">
                <input class="col-xs-6" type="time" value={hour.begin.getHours()+":"+hour.begin.getMinutes()} />
                <input class="col-xs-6" type="time" value={hour.end.getHours()+":"+hour.end.getMinutes()} />
                <div className="col-xs-12" style="min-width: 40px;">
                  {(ctrl.isLastElement(day,hour) && indexHour>0) && <button type="button" className="btn btn-danger" onClick={()=>onDelete(day, indexDay,hour, indexHour)}><span
    className="glyphicon glyphicon-remove"/></button>}
                  {(ctrl.isLastElement(day,hour)) && <button type="button" className="btn btn-primary" onClick={()=>onAdd(day,indexDay)}>
                    <span className="glyphicon glyphicon-plus"/>
                  </button>}
                </div>
              </div>}</>)}
            </div>
          </div>
        </div>
      </div>)}</>
    default:
      return <>{element.data.days.map((day, indexDay) => <div className="form-group">
        <label className="col-sm-2 col-md-2 col-xs-12 control-label">{day.label}</label>
        <div className="col-sm-10 col-md-9 col-xs-12">
          <div className="row">
            <div className="btn-group col-xs-12 col-sm-3">
              <YesNo yes={day.isOpen} onChange={(isOpen) => onOpen(day, indexDay, isOpen)}/>
            </div>
            <div className="col-xs-12 col-sm-9">
              {day.hours.map((hour, indexHour) =><>{day.isOpen && <div className="row" >
                <input className="col-xs-6 col-sm-5" type="time" value={hour.begin.getHours() + ":" + hour.begin.getMinutes()}/>
                <input className="col-xs-6 col-sm-5" type="time" value={hour.end.getHours() + ":" + hour.end.getMinutes()}/>
              
                <div className="col-xs-12 col-sm-2" style="min-width: 40px;">
                  {(ctrl.isLastElement(day,hour) && indexHour>0) && <button type="button" className="btn btn-danger" onClick={()=>onDelete(day, indexDay, hour,indexHour)}><span className="glyphicon glyphicon-remove"></span></button>}
                  {ctrl.isLastElement(day,hour)&& <button type="button" className="btn btn-primary" onClick={() =>onAdd(day,indexDay)}><span className="glyphicon glyphicon-plus"></span>
                  </button>}
                </div>
              </div>}</>)}
            </div>
          </div>
        </div>
      </div>)}</>
  }
}

export const ElementHoursAdmin = ({ element, mode, onChange }) => {

  const ctrl = {};
  
  const onChangeTime = (day, indexDay, hour, indexHour, value, isBeginDate) => {

    const values = value.split(":");

    const newHour = {...hour, begin:{...hour.begin}, end:{...hour.end}};
    
    if(isBeginDate){
          
        }
  
  }
  
  const onOpen = (day, indexDay, isOpen) => {
    const newDay = {...day, isOpen}
    const days = [...element.data.days];
    days[indexDay] = newDay;
    onChange({ what: "element-edit", element: {...element, data: {days}}});
  }

  const onAdd = (day, indexDay) => {
    const beginDate = new Date();
    beginDate.setHours(8);
    beginDate.setMinutes(0);

    const endDate = new Date();
    endDate.setHours(18);
    endDate.setMinutes(30);

    const newDay = {...day, hours:[...day.hours]}
    newDay.hours.push({
      begin: beginDate,
      end: endDate,
    });
    
    const days = [...element.data.days];
    days[indexDay] = newDay;
    onChange({ what: "element-edit", element: {...element, data: {days}}});
  };

  const onDelete = function(day, indexDay, hour, indexHour) {
    const newDay = {...day, hours:[...day.hours]}
    newDay.hours.splice(indexHour, 1);
    const days = [...element.data.days];
    days[indexDay] = newDay;
    onChange({ what: "element-edit", element: {...element, data: {days}}});
  };

  ctrl.isLastElement = (day, indexHour) => {
    return indexHour === day.hours.length - 1;
  };

  ctrl.getClassLabel = (element) => {
    if (element.$level <= 2) {
      return 'col-sm-2 col-md-2 col-xs-12';
    }
    return 'col-sm-12 col-md-12 col-xs-12';
  };

  ctrl.getClassField = (element) => {
    if (element.$level <= 2) {
      return 'col-sm-10 col-md-9 col-xs-12';
    }
    return 'col-sm-12 col-md-12 col-xs-12';
  };
  
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Horaires'}
          adminEdit={<HoursAdmin
              ctrl={ctrl}
              onAdd={onAdd}
              onDelete={onDelete}
              onOpen={onOpen}
              element={element}
          />}
          adminView={<DaysHours element={element} />}>
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementHoursAdmin, ['element', 'mode', 'onChange']));

export default name;
