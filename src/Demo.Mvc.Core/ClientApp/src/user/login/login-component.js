import app from '../../app.module';
import redux from '../../redux';
import { page } from '../../shared/services/page-factory';
import React, {useEffect} from 'react';
import { react2angular } from 'react2angular';
import {free} from "../../free/free-factory";
import LoginBworld from "./loginBworld-component";
import LoginSocial from "./loginSocial-component";

const name = 'login';

const Login = () => {

  useEffect(() =>{
    page.setTitle('Authentification');
  });
  
  return (
      <div className="row">
        <div className="col-md-10 col-md-offset-1 col-xs-12 col-xs-offset-0">
          <h1>Connexion</h1>
          <div>
            <p>Connectez-vous en quelques clics, c'est simple et rapide! Vous pouvez associer plusieurs type de login
              dans les paramètres utilisateurs.</p>
            <LoginBworld/>
            <hr/>
            <LoginSocial />
          </div>
        </div>
      </div>
  );
};

app.component(name, react2angular(Login, []));

export default name;