import app from '../../app.module';
import {ElementAdmin} from "../elementAdmin-component";
import {react2angular} from "react2angular";
import React from "react";
import {DaysHours} from "./elementHours-component";

const name = 'elementHoursAdmin';

const YesNo =({yes, onChange, name}) =>{
  
  return <><label>Ouvert
    <input type="radio" name={name} checked={yes} onChange={() => onChange(true)}/></label>
 <label>Fermé
    <input type="radio" name={name} checked={!yes} onChange={() => onChange(false)}/>
 </label></>
  
}

const HoursAdmin = ({ctrl, element, onAdd, onDelete, onOpen, onChangeTime}) => {
  const level = element.$level.toString();
  
  switch (level) {
    case "3":
      return <>{element.data.days.map((day, indexDay) => <div className="form-group" key={day.label}>
        <label className="col-xs-12 control-label">{day.label}</label>
        <div className=" col-xs-12">
          <div className="row">
            <div className="btn-group col-xs-12">
              <YesNo yes={day.isOpen} onChange={(isOpen) => onOpen(day, indexDay, isOpen)} name={element.property+"_"+day.label}/>
            </div>
            <div className="col-xs-12">
              {day.hours.map((hour, indexHour) =><>
              {day.isOpen && <div className="row" key={indexHour}>
                <input className="col-xs-6" type="time" value={hour.begin.getHours()+":"+hour.begin.getMinutes()}  onChange={(e)=>onChangeTime(day, indexDay, hour, indexHour,  e.target.value, true)} />
                <input className="col-xs-6" type="time" value={hour.end.getHours()+":"+hour.end.getMinutes()}  onChange={(e)=>onChangeTime(day, indexDay, hour, indexHour,  e.target.value, false)} />
                <div className="col-xs-12" style={{'minWidth': '40px'}}>
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
      return <>{element.data.days.map((day, indexDay) => <div key={day.label} className="form-group">
        <label className="col-sm-2 col-md-2 col-xs-12 control-label">{day.label}</label>
        <div className="col-sm-10 col-md-9 col-xs-12">
          <div className="row">
            <div className="btn-group col-xs-12 col-sm-3">
              <YesNo yes={day.isOpen} onChange={(isOpen) => onOpen(day, indexDay, isOpen)} name={element.property+"_"+day.label}/>
            </div>
            <div className="col-xs-12 col-sm-9">
              {day.hours.map((hour, indexHour) =><>{day.isOpen && <div key={indexHour} className="row" >
                <input className="col-xs-6 col-sm-5" type="time" value={hour.begin.getHours() + ":" + hour.begin.getMinutes()} onChange={(e)=>onChangeTime(day, indexDay, hour, indexHour,  e.target.value, true)} />
                <input className="col-xs-6 col-sm-5" type="time" value={hour.end.getHours() + ":" + hour.end.getMinutes()}  onChange={(e)=>onChangeTime(day, indexDay, hour, indexHour,  e.target.value, false)}/>
              
                <div className="col-xs-12 col-sm-2" style={{'minWidth': '40px'}}>
                  {(ctrl.isLastElement(day,hour) && indexHour>0) && <button type="button" className="btn btn-danger" onClick={()=>onDelete(day, indexDay, hour,indexHour)}><span className="glyphicon glyphicon-remove"/></button>}
                  {ctrl.isLastElement(day,hour)&& <button type="button" className="btn btn-primary" onClick={() =>onAdd(day,indexDay)}><span className="glyphicon glyphicon-plus"/>
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
    const newHour = {...hour, begin:hour.begin, end:hour.end};
    let date = isBeginDate ? newHour.begin : newHour.end
    date.setHours(parseInt(values[0],10));
    date.setMinutes(parseInt(values[1],10));

    const newDay = {...day, hours:[...day.hours]}
    newDay.hours[indexHour] = newHour;
    const days = [...element.data.days];
    days[indexDay] = newDay;
    onChange({ what: "element-edit", element: {...element, data: {days}}});
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
              onChangeTime={onChangeTime}
          />}
          adminView={<DaysHours element={element} />}>
      </ElementAdmin>
  );
};