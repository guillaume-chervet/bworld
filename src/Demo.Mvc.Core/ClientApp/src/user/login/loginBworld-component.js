import {master} from '../../shared/providers/master-provider';
import {login} from './login-service';
import $http from '../../http';
import React, {useReducer} from 'react';
import {
  getMessageStatus,
  initMessages, formReducer,
  isFormValid,
  state,
} from '../../elements/message/elementMessage-component';

const rules = {
  email: ['required', 'email'],
  password: login.rules.password,
};

const newForm = () => {return {
  email: {value : '', message:'', rules : rules.email, state: {...state}, isVisible:true },
  password: {value : '', message:'', rules : rules.password, state: {...state}, isVisible:true},
}};

const initialState = {
  isSubmited: false,
  form : newForm()
};

initialState.form = initMessages(initialState.form);

const reducer = (formPropertyName) => (state, action) => {
  switch (action.type) {
    case 'error': {
      return {...state, [formPropertyName]: {...state[formPropertyName], email: {...state[formPropertyName].email, message: "Login ou mot de passe non valide" } } };
    }
    default:
      return formReducer([formPropertyName])(state,  action);
  }
};

const submit = (form, dispatch) => {
  if (!isFormValid(form)) {
    return;
  }
  const dataToSend = {
    email: form.email.value,
    password: form.password.value,
    rememberMe: true,
  };

  return $http
      .post(master.getUrl('Account/Login'), dataToSend)
      .then(function(response) {
        const result = response.data;
        if (result.isSuccess) {
          window.location = login.getReturnUrl();
        } else {
          // TODO passer en camel case le retour MVC
          const errors = result.validationResult.errors;
          if (errors.invalidLogin) {
            dispatch({type:'error'});
          }
        }

        return response.data;
      });
};

const LoginBworld = () => {

  const [state, dispatch] = useReducer(reducer("form"), initialState);
  const onChange = (e) => { dispatch({type: 'onChange', data: {
      name: e.target.name,
      value: e.target.value,
    } }) };
  const onBlur = (e) => { dispatch({type: 'onBlur', data: {
      name: e.target.name,
    } }) };
  const onFocus = (e) => { dispatch({type: 'onFocus', data: {
      name: e.target.name,
    } }) };
  const onSubmit = (e) => {
    e.preventDefault();
    dispatch({type: 'onSubmit'});
    submit(state.form, dispatch);
  };
  
  const getNavCreate = function() {
    let returnUrl = login.formatReturnUrl(login.getReturnUrl());
    return `/utilisateur/creation?dm=false&returnUrl=${returnUrl}`;
  };
  const events = {onBlur, onChange, onFocus};
  const status = getMessageStatus(state.form, state.isSubmited);
  return (
      <div className="row">
        <div className="col-sm-2" />
        <div className="col-sm-8 mw-login text-center">

          <div className="panel panel-default">
            <div className="panel-heading">
              <h2>Connexion <span className="glyphicon glyphicon-globe" aria-hidden="true"/></h2>
            </div>
            <div className="panel-body">
              <form name="formPerso" role="form" className="form-horizontal" encType="multipart/form-data" noValidate
                    onSubmit={onSubmit} >
                <div className="row">
                  <div className={"form-group form-group-lg "+ status.email.className} >
                    <label className="col-sm-3 control-label">Email : </label>
                    <div className="col-sm-9 col-md-7">
                      <input type="text" name="email" value={state.form.email.value} {...events} className="form-control"/>
                      <span className="error-block">{status.email.message}</span>
                    </div>
                  </div>
                  <div className={"form-group form-group-lg "+ status.password.className} >
                    <label className="col-sm-3 control-label">Mot de passe : </label>
                    <div className="col-sm-9 col-md-7">
                      <input type="password" name="password" value={state.form.password.value} {...events} className="form-control" />
                      <span className="error-block">{status.password.message}</span>
                    </div>
                  </div>
                  <div className="form-group form-group-lg">
                    <div className="col-sm-6">
                      <a href={getNavCreate()}>Créer un compte</a>
                    </div>
                    <div className="col-sm-6">
                      <a href="/utilisateur/reset-password?dm=false">Mot de passe perdu</a>
                    </div>
                  </div>
                  <div className="form-group">
                    <div className="col-sm-offset-3 col-sm-9">
                      <button type="submit" className="btn btn-lg btn-success"><span
                          className="glyphicon glyphicon-ok" /> Valider
                      </button>
                    </div>
                  </div>
                </div>
              </form>
            </div>

          </div>

          <div className="col-sm-2" />
        </div>
      </div>
  );
};

export default LoginBworld;
