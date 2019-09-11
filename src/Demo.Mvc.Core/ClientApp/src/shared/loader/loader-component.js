import { react2angular } from "react2angular";
import app from '../../app.module';
import { withStore } from "../../reducers.config";
import React from "react";
import { connect } from 'react-redux'

import './loader.css';

const name = 'loader';

const Loader = ({ isLoading, message }) => {
  
  if(!isLoading){
    return null;
  }

  return (<div id="loading-layer">
    <div id="loading">
      <div id="circularG">
        <div id="circularG_1" className="circularG"></div>
        <div id="circularG_2" className="circularG"></div>
        <div id="circularG_3" className="circularG"></div>
        <div id="circularG_4" className="circularG"></div>
        <div id="circularG_5" className="circularG"></div>
        <div id="circularG_6" className="circularG"></div>
        <div id="circularG_7" className="circularG"></div>
        <div id="circularG_8" className="circularG"></div>
      </div>
      <span>{message}</span>
    </div>
  </div>);
};


const mapStateToProps = (state, ownProps) => {
  return {
    isLoading: state.loader.isLoading,
    message: state.loader.message ?? 'Chargement...'
  };
};

const mapDispatchToProps = (dispatch, ownProps) => {
  return {};
};

const LoaderWithState = withStore(connect(
    mapStateToProps,
    mapDispatchToProps
)(Loader));

app.component(name, react2angular(LoaderWithState, []));

export default name;