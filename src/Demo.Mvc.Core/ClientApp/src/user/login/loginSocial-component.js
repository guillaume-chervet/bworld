import { login } from './login-service';
import React from 'react';

const LoginSocial = () => {

  const returnUrl = login.getReturnUrl();
  const postUrl = login.getPostUrl();
  const domainLoginUrl = login.domainLoginUrl;
  
  return (
      <div className="row">
        <div className="col-sm-2"></div>
        <div className="col-sm-8 mw-login text-center">

          <div className="panel panel-default">
            <div className="panel-heading">
              <h2>Connexion <a href={domainLoginUrl} alt="Système d'authentification centralisé"><span
                  className="glyphicon glyphicon-globe" aria-hidden="true"></span> bworld</a> via les réseaux sociaux
              </h2>
            </div>
            <div className="panel-body">
              <form action={postUrl} method="post" noValidate="novalidate">
                <input type="hidden" name="returnUrl" value={returnUrl}/>
                <div className="row">
                  <div className="col-sm-6 mw-login-panel">
                    <button type="submit" className="btn-lg btn-social btn-google" id="Google" name="provider"
                            value="Google" title="Connexion avec votre compte Google">
                      <i className="fa fa-google"></i>Google
                    </button>
                  </div>
                  <div className="col-sm-6 mw-login-panel">
                    <button type="submit" className="btn-lg btn-social btn-facebook" id="Facebook" name="provider"
                            value="Facebook" title="Connexion avec votre compte Facebook">
                      <i className="fa fa-facebook"></i> Facebook
                    </button>
                  </div>
                  <div className="col-sm-6 mw-login-panel">
                    <button type="submit" className="btn-lg btn-social btn-microsoft" id="Microsoft" name="provider"
                            value="Microsoft" title="Connexion avec votre compte Microsoft">
                      <i className="fa fa-windows"></i> Microsoft
                    </button>
                  </div>
                  <div className="col-sm-6 mw-login-panel">
                    <button type="submit" id="Twitter" name="provider" value="Twitter"
                            className="btn-lg btn-social btn-twitter" title="Connexion avec votre compte Twitter">
                      <i className="fa fa-twitter"></i>
                      Twitter
                    </button>
                  </div>
                </div>
              </form>
              <p>Si vous n'êtes pas inscrit, cliquer sur l'un des boutons afin de commencer.</p>
            </div>

          </div>

          <div className="col-sm-2"></div>
        </div>
      </div>
  );
};


export default LoginSocial;