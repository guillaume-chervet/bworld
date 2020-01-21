import app from '../../../app.module';
import history from '../../../history';
import { page } from '../../../shared/services/page-factory';
import React, { useEffect } from 'react';
import { react2angular } from 'react2angular';

const name = 'externalLoginFailure';

const ExternalLoginFailure = () => {
  const goHome = () => {
    history.search({ dm: null }, '/');
  };

  useEffect(() => {
    page.setTitle('Association compte échec');
  });

  return (
    <div className="row">
      <div className="col-md-10 col-md-offset-1 col-xs-12 col-xs-offset-0">
        <h1>Echec lors de l'association de votre compte</h1>
        <p>
          Il nous a été impossible de vous authentifiez. Si cette erreur
          persiste veuillez contacter un administrateur.
        </p>
        <div className="text-center">
          <button
            type="button"
            className="btn btn-primary btn-lg"
            onClick={goHome}>
            Page d'accueil
          </button>
        </div>
      </div>
    </div>
  );
};

app.component(name, react2angular(ExternalLoginFailure, []));

export default name;
