import app from '../../app.module';
import history from '../../history';
import { user } from '../info/user-factory';
import { login } from '../login/login-service';
import { react2angular } from "react2angular";
import { withStore } from "../../reducers.config";
import React from "react";
import { connect } from 'react-redux'
import {Empty} from "../../shared/directives/empty-component";

const name = 'associationLogin';

const hasLoginInternal = (logins, provider) => {
  for (let j = 0; j < logins.length; j++) {
    if (logins[j] === provider) {
      return true;
    }
  }
  return false;
};

const AssociationLogin = ({ logins}) => {

  const hasLogin = provider => hasLoginInternal(logins, provider);
  const deleteUserLogin = (provider) => user.deleteUserLoginAsync(provider);
  const returnUrl = login.getFullBaseUrl() + history.path();
  const postUrl = login.getPostAssociationUrl();

  return (
      <div>
        <h2>Fournisseurs d'identité associés à votre compte</h2>

        <Empty items="logins" content="'Aucun fournisseurs associés.'" />

        {logins.length >0 ? <table className="table table-striped">
          <thead>
          <tr>
            <th>Fournisseur(s)</th>
            <th>Supprimer association(s)</th>
          </tr>
          </thead>
          <tbody>
          {logins.map(login => (<tr>
            <td><span>{login}</span></td>
            <td>
              <button disabled={logins.length <= 1} type="button" className="btn btn-lg btn-danger"
                      onClick={() =>deleteUserLogin(login)}><span className="glyphicon glyphicon-remove"></span>
              </button>
            </td>
          </tr>))}
          </tbody>
        </table>:null}

        {logins.length < 4 ? <div>
          <p>Associer votre compte à d'autres fournisseurs d'identité. Cela vous permettra de vous connectez via ces
            différents founisseurs.</p>
          <form action={postUrl} method="post" noValidate="novalidate">
            <input type="hidden" name="returnUrl" value={returnUrl}/>
            <p>
              {!hasLogin('Google') ? <button type="submit" className="btn btn-lg btn-social btn-google"
                      id="Google" name="provider" value="Google" title="Connexion avec votre compte Google">
                <i className="fa fa-google"></i>Google
              </button> : null }
              {!hasLogin('Facebook') ? <button type="submit" className="btn btn-lg btn-social btn-facebook"
                      id="Facebook" name="provider" value="Facebook" title="Connexion avec votre compte Facebook">
                <i className="fa fa-facebook"></i>Facebook
              </button> : null}
              {!hasLogin('Microsoft') ? <button type="submit" className="btn btn-lg btn-social btn-microsoft"
                      id="Microsoft" name="provider" value="Microsoft" title="Connexion avec votre compte Microsoft">
                <i className="fa fa-windows"></i>Microsoft
              </button> : null}
              {!hasLogin('Twitter') ? <button type="submit" id="Twitter" name="provider" value="Twitter"
                      className="btn btn-lg btn-social btn-twitter" title="Connexion avec votre compte Twitter">
                <i className="fa fa-twitter"></i>
                Twitter
              </button> : null}
            </p>
          </form>
        </div> : null }
      </div>
  );
};


const mapStateToProps = (state, ownProps) => {
  return {
    logins: state.user.logins
  };
};

const mapDispatchToProps = (dispatch, ownProps) => {
  return {};
};

const AssociationLoginWithState = withStore(connect(
    mapStateToProps,
    mapDispatchToProps
)(AssociationLogin));

app.component(name, react2angular(AssociationLoginWithState, []));

export default name;
