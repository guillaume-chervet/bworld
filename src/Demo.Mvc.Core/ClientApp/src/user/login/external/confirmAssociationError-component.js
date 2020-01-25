import app from '../../../app.module';
import history from '../../../history';
import { page } from '../../../shared/services/page-factory';
import React, { useEffect } from 'react';
import { react2angular } from 'react2angular';

const name = 'confirmAssociationError';

const ConfirmationAssociationError = () => {
  const goHome = () => {
    history.search({ dm: null, returnUrl: null, provider: null }, '/');
  };
  const searchObject = history.search();
  const provider = searchObject.provider;
  useEffect(() => {
    page.setTitle('Echec association provider');
  });
  return (
    <div className="row">
      <div className="col-md-10 col-md-offset-1 col-xs-12 col-xs-offset-0">
        <h1>Echec lors de l'association de votre compte {provider}</h1>
        <p>
          Il nous a été impossible d'associer votre compte utilisateur avec{' '}
          {provider}. Si cette erreur persiste veuillez contacter un
          administrateur.
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

app.component(name, react2angular(ConfirmationAssociationError, []));

export default name;
