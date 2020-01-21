import app from '../../../app.module';
import { page } from '../../../shared/services/page-factory';
import { externalLogin } from '../external/externalLogin-service';
import React, { useEffect } from 'react';
import { react2angular } from 'react2angular';
import CreateAccount from './createAccount-component';

const name = 'pageCreateAccount';

const Login = () => {
  useEffect(() => {
    page.setTitle('Création compte utilisateur');
    externalLogin.init();
  });
  return (
    <div className="row">
      <div className="col-md-10 col-md-offset-1 col-xs-12 col-xs-offset-0">
        <h1>Création de votre compte utilisateur bworld</h1>
        <h2>Vos informations d'identification</h2>
        <CreateAccount />
      </div>
    </div>
  );
};

app.component(name, react2angular(Login, []));

export default name;
