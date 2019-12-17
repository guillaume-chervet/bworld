import app from '../../../app.module';
import { master } from '../../../shared/providers/master-provider';
import { externalLogin } from '../external/externalLogin-service';
import { login } from '../login-service';
import { toast as toastr } from '../../../shared/services/toastr-factory';
import $http from '../../../http';

import React, {useEffect, useReducer} from 'react';
import {
  getMessageStatus,
  initMessages, formReducer,
  isFormValid,
  state,
} from '../../../elements/message/elementMessage-component';
import {react2angular} from "react2angular";

const name = 'createAccount';

const submitAsync = function(form, dataToSend) {
  const returnUrl = externalLogin.getReturnUrl(
      externalLogin.externalLogin.returnUrl
  );
  form.uEmail.mw.setValidity('EMAIL', true);
  return $http
      .post(master.getUrl('Account/Register'), dataToSend)
      .then(function(response) {
        const result = response.data;
        if (result.isSuccess) {
          toastr.success(
              'Creation réalisé avec succès. Un email de confirmation vous à été envoyé. Vous allez être redirigé vers la page initiale.',
              'Confirmation utilisateur'
          );
          window.location = returnUrl;
        } else {
          // TODO passer en camel case le retour MVC
          const errors = result.validationResult.errors;
          if (errors.email) {
            form.uEmail.mw.setValidity('EMAIL', false, errors.Email.Message);
          }
        }

        return response.data;
      });
};

const rules = {
  condition: [
    'required',
    {
      equal: {
        equal: true,
        message: 'Vous devez accèpter les règles de confidentialité.',
      },
    },
  ],
  email: ['required', 'email'],
  lastName: 'required',
  firstName: 'required',
};

const newForm = () => {return {
  email: {value : '', message:'', state: {...state}, isVisible:true, rules:rules.email },
  password: {value : '', message:'', state: {...state}, isVisible:true, rules:[] },
  passwordConfirm: {value : '', message:'', state: {...state}, isVisible:true, rules:[] },
  lastName: {value : '', message:'', state: {...state}, isVisible:true, rules:rules.lastName },
  firstName: {value : '', message:'', state: {...state}, isVisible:true, rules: rules.firstName },
  condition: {value : null, message:'', state: {...state}, isVisible:true, rules: rules.condition },
}};

const initialState = {
  isSubmited: false,
  passwordHided:true,
  form : newForm()
};

initialState.form = initMessages(initialState.form);

const reducer = (formPropertyName) => (state, action) => {
  switch (action.type) {
    default:
      const validatePassword = () => {
        if (state.form.password.value === state.form.passwordConfirm.value) {
          return { success: true, message: '' };
        }
        return {
          success: false,
          message: 'Les deux mot de passe doivent être identique.',
        };
      };
      const customPassword = {
        custom: {
          validateView: validatePassword,
          validateModel: validatePassword,
        },
      };
      const rules = {
        password: login.rules.password,
        passwordConfirm: ['required', customPassword],
      };
      const newState = {...state, form : { ...state.form, password: { ...state.form.password, rules : rules.password }} };
      return formReducer(formPropertyName)(newState,  action);
  }
};

const submit = (form) => {
  if (!isFormValid(form)) {
    return;
  }
  const dataToSend = {
    email: form.email,
    firstName: form.firstName,
    lastName: form.lastName,
    password: form.password,
  };
  return submitAsync(form, dataToSend);

};

const CreateAccount = () => {
  const [state, dispatch] = useReducer(reducer("form"), initialState);
  const password = () => {
    dispatch({type: 'password'});
  };
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
    submit(state.form);
  };
  const inputType = state.passwordHided ? "password" : "text";
  const events = {onBlur, onChange, onFocus};
  const status = getMessageStatus(state.form, state.isSubmited);
  return (<form role="form" name="form" className="form-horizontal" encType="multipart/form-data" noValidate
                submit={submit}>

      <div className={"form-group form-group-lg "+ status.password.className}>
              <label htmlFor="password" className="col-sm-3 control-label">Mot de passe: </label>
              <div className="col-sm-4">
                <input  id="password" type={inputType} name="password" value={state.form.password.value} {...events} className="form-control" />
                <span className="error-block">{status.password.message}</span>
              </div>
              <div className="col-sm-5">
                <button type="button" onClick={password} className="btn btn-lg btn-default">{state.passwordHided ? "Afficher" : "Masquer"}
                </button>
              </div>
            </div>
            <div className={"form-group form-group-lg "+ status.passwordConfirm.className}>
              <label htmlFor="passwordConfirm" className="col-sm-3 control-label">Confirmation du mot de passe: </label>
              <div className="col-sm-4">
                <input id="passwordConfirm" type={inputType} name="passwordConfirm"  value={state.form.passwordConfirm.value} {...events} className="form-control" />
                <span className="error-block">{status.passwordConfirm.message}</span>
              </div>
            </div>

            <div className={"form-group form-group-lg "+ status.lastName.className}>
              <label htmlFor="LastName" className="col-sm-3 control-label">Nom: </label>
              <div className="col-sm-4">
                <input id="LastName" type="text" name="lastName" value={state.form.lastName.value} {...events} className="form-control" />
                <span className="error-block">{status.lastName.message}</span>
              </div>
            </div>
            <div className={"form-group form-group-lg "+ status.firstName.className}>
              <label htmlFor="FirstName" className="col-sm-3 control-label">Prénom: </label>
              <div className="col-sm-4">
                <input id="FirstName" type="text" name="firstName" value={state.form.firtName.value} {...events} className="form-control" />
                <span className="error-block">{status.firstName.message}</span>
              </div>
            </div>
            <div className={"form-group form-group-lg "+ status.condition.className}>
              <div className="checkbox col-sm-offset-3 col-sm-9">
                <label>
                  <input name="uCondition" type="checkbox" value={state.form.condition.value} {...events}>
                    J'accepte les <a href="https://www.bworld.fr/page/5c34885b-01ee-4436-99b6-adbf36c467dc/regles-de-confidentialite?display_menu=false"  target="_blank">règles de confidentialité</a>
                  </input>
                </label>
                <span className="error-block">{status.condition.message}</span>
              </div>
            </div>

            <div>
              <hr/>
              <div className="form-group form-group-lg">
                <div className="col-sm-offset-3 col-sm-9">
                  <button type="submit" className="btn btn-lg btn-success"><span className="glyphicon glyphicon-ok"/> Inscription
                  </button>
                </div>
              </div>
            </div>
          </form>);
  
};

export default CreateAccount;