import app from '../../app.module';
import { react2angular } from 'react2angular';
import { withStore } from '../../reducers.config';
import React from 'react';
import { connect } from 'react-redux';

const name = 'mwAlert';

const Alert = ({ alerts }) => {
  if (alerts && alerts.length <= 0) {
    return null;
  }
  return (
    <div>
      <span>
        {alert.type} : {alert.msg}
      </span>
    </div>
  );
};

const mapStateToProps = state => {
  return {
    alerts: state.master.menuData.isDisplayMenu ? state.user.alerts : null,
  };
};

const AlertWithState = withStore(connect(mapStateToProps)(Alert));

app.component(name, react2angular(AlertWithState, []));

export default name;
