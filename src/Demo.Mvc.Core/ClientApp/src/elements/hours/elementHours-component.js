import app from '../../app.module';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import moment from 'moment';

import './hours.css';

const getTime = date1 => {
  const momentObj = moment(date1);
  return `${momentObj.format('HH')}h${momentObj.format('mm')}`;
};

const Hours = ({ hours }) => {
  return (
    <ul>
      {hours.map(hour => (
        <li>
          {getTime(hour.begin)}-{getTime(hour.end)}
        </li>
      ))}
    </ul>
  );
};

export const DaysHours = ({element}) => {
    return (
      <table>
        <tbody>
          {element.data.days.map(day => (
            <tr>
              <td>{day.label}</td>
              <td>{!day.isOpen ? 'Fermé' : ''}</td>
              <td>
                <Hours hours={day.hours} />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    );
};

const name = 'elementHours';

app.component(name, react2angular(DaysHours, ['element']));

export default name;
