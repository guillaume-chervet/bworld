import app from '../../../app.module';
import history from '../../../history';
import { page } from '../../../shared/services/page-factory';
import { master } from '../../../shared/providers/master-provider';
import { login } from '../login-service';
import { toast as toastr } from '../../../shared/services/toastr-factory';
import $http from '../../../http';
import React, { useEffect, useReducer } from 'react';
import {
  getMessageStatus,
  initMessages,
  formReducer,
  isFormValid,
  state,
} from '../../../elements/message/elementMessage-component';
import { react2angular } from 'react2angular';

const name = 'reinitPassword';

const submitAsync = (form, dataToSend) => {
  return $http
    .post(master.getUrl('Account/ChangePassword'), dataToSend)
    .then(function(response) {
      const result = response.data;
      if (result.isSuccess) {
        toastr.success(
          'Changement du mot de passe réalisé avec succès. Vous allez être redirigé vers la page initiale.',
          'Confirmation utilisateur'
        );
        history.path('/utilisateur/connexion');
      }

      return response.data;
    });
};

const newForm = () => {
  return {
    email: {
      value: '',
      message: '',
      state: { ...state },
      isVisible: true,
      rules: ['required', 'email'],
    },
    password: {
      value: '',
      message: '',
      state: { ...state },
      isVisible: true,
      rules: [],
    },
    passwordConfirm: {
      value: '',
      message: '',
      state: { ...state },
      isVisible: true,
      rules: [],
    },
  };
};

const initialState = {
  isSubmited: false,
  passwordHided: true,
  form: newForm(),
};

initialState.form = initMessages(initialState.form);

const reducer = formPropertyName => (state, action) => {
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

      const newState = {
        ...state,
        form: {
          ...state.form,
          password: { ...state.form.password, rules: rules.password },
          passwordConfirm: {
            ...state.form.passwordConfirm,
            rules: rules.passwordConfirm,
          },
        },
      };

      return formReducer(formPropertyName)(newState, action);
  }
};

const submit = form => {
  if (!isFormValid(form)) {
    return;
  }
  const search = history.search();
  const dataToSend = {
    userId: search.userId,
    token: search.token,
    password: form.password.value,
  };
  return submitAsync(form, dataToSend);
};

const ReinitPassword = () => {
  useEffect(() => {
    page.setTitle('Ré-initialisation mot de passe');
  });
  const [state, dispatch] = useReducer(reducer('form'), initialState);

  const password = () => {
    dispatch({ type: 'password' });
  };

  const onChange = e => {
    dispatch({
      type: 'onChange',
      data: {
        name: e.target.name,
        value: e.target.value,
      },
    });
  };
  const onBlur = e => {
    dispatch({
      type: 'onBlur',
      data: {
        name: e.target.name,
      },
    });
  };
  const onFocus = e => {
    dispatch({
      type: 'onFocus',
      data: {
        name: e.target.name,
      },
    });
  };
  const onSubmit = e => {
    e.preventDefault();
    dispatch({ type: 'onSubmit' });
    submit(state.form);
  };
  const inputType = state.passwordHided ? 'password' : 'text';
  const events = { onBlur, onChange, onFocus };
  const status = getMessageStatus(state.form, state.isSubmited);
  return (
    <div className="row">
      <div className="col-md-10 col-md-offset-1 col-xs-12 col-xs-offset-0">
        <h1>Ré-initialisation mot de passe</h1>
        <h2>Vos nouvelles informations</h2>
        <form
          role="form"
          name="form"
          className="form-horizontal"
          encType="multipart/form-data"
          noValidate
          onSubmit={onSubmit}>
          <div
            className={'form-group form-group-lg ' + status.password.className}>
            <label htmlFor="password" className="col-sm-3 control-label">
              Mot de passe:{' '}
            </label>
            <div className="col-sm-4">
              <input
                id="password"
                type={inputType}
                name="password"
                value={state.form.password.value}
                {...events}
                className="form-control"
              />
              <span className="error-block">{status.password.message}</span>
            </div>
            <div className="col-sm-5">
              <button
                type="button"
                onClick={password}
                className="btn btn-lg btn-default">
                {state.passwordHided ? 'Afficher' : 'Masquer'}
              </button>
            </div>
          </div>
          <div
            className={
              'form-group form-group-lg ' + status.passwordConfirm.className
            }>
            <label htmlFor="passwordConfirm" className="col-sm-3 control-label">
              Confirmation du mot de passe:{' '}
            </label>
            <div className="col-sm-4">
              <input
                id="passwordConfirm"
                type={inputType}
                name="passwordConfirm"
                value={state.form.passwordConfirm.value}
                {...events}
                className="form-control"
              />
              <span className="error-block">
                {status.passwordConfirm.message}
              </span>
            </div>
          </div>

          <div>
            <hr />
            <div className="form-group form-group-lg">
              <div className="col-sm-offset-3 col-sm-9">
                <button type="submit" className="btn btn-lg btn-warning">
                  <span className="glyphicon glyphicon-ok" /> Changer
                </button>
              </div>
            </div>
          </div>
        </form>
      </div>
    </div>
  );
};

app.component(name, react2angular(ReinitPassword, []));

export default name;
