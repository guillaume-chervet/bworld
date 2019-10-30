import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import React, {useEffect} from 'react';
import { react2angular } from 'react2angular';

const name = 'confirmEmailError';

const ConfirmEmailError = () => {
  useEffect(() =>{
    page.setTitle('Confirmation email échec');
  });
  const searchObject = history.search();
  const user = {};
  if (searchObject.email) {
    user.email = searchObject.email;
  }
  const page = {
    provider: searchObject.provider,
  };
  const goHome = () =>  history.search({'email': null, 'dm': null}, '/');
  return (
      <div className="row">
        <div className="col-md-10 col-md-offset-1 col-xs-12 col-xs-offset-0">
          <h1>Votre email n'a pas pu être confirmé</h1>
          <p>Il nous a été impossible de valider votre email. Si cette erreur persiste veuillez contacter un
            administrateur.</p>
          <div className="text-center">
            <button type="button" className="btn btn-primary btn-lg" onClick={goHome}>Page d'accueil</button>
          </div>
        </div>
      </div>
  );
};

app.component(name, react2angular(ConfirmEmailError, []));

export default name;
