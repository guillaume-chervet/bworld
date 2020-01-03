import app from '../../app.module';
import history from '../../history';
import React, {useEffect} from 'react';
import { react2angular } from 'react2angular';

const name = 'confirmEmail';

const ConfirmEmail = () => {

  useEffect(() =>{
    page.setTitle('Confirmation email');
  });
  
  const searchObject = history.search();

  const user = {};
  if (searchObject.email) {
    user.email = searchObject.email;
  }

  const page = {
    provider: searchObject.provider,
  };

  const goHome = () => history.search({'dm': null}, {'email': null}, '/');
  return (

      <div className="row">
        <div className="col-md-10 col-md-offset-1 col-xs-12 col-xs-offset-0">
          <h1>Votre email a été confirmé avec succès.</h1>
          <p>Vous pouvez désormais naviguer sur le site à votre guise.</p>
          <div className="text-center">
            <button className="btn btn-primary btn-lg" onClick={goHome}>Page d'accueil</button>
          </div>
        </div>
      </div>
  );
};

app.component(name, react2angular(ConfirmEmail, []));

export default name;
