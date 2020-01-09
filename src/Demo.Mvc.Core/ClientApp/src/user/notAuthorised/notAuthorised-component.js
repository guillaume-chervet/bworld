import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import React, {useEffect} from 'react';
import { react2angular } from 'react2angular';

const name = 'notAuthorised';

const NotAuthorised = () => {

  useEffect(() =>{
    page.setTitle('Non authorisé');
  });

  const goHome = () => history.search({'dm': null}, '/');

  return (
      <div className="row">
        <div className="col-md-10 col-md-offset-1 col-xs-12 col-xs-offset-0">
          <h1>Non authorisé</h1>
          <p>Vous n'avez pas les authorisations pour accèder à cette ressource.</p>
          <div className="text-center">
            <button type="button" className="btn btn-primary btn-lg" onClick={goHome}>Page d'accueil</button>
          </div>
        </div>
      </div>
  );
};

app.component(name, react2angular(NotAuthorised, []));

export default name;