import app from '../../../app.module';
import { page } from '../../../shared/services/page-factory';
import { master } from '../../../shared/providers/master-provider';
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

const name = 'resetPassword';

const rules = {
  email: ['required', 'email'],
};

const newForm = () => {
  return {
    email: {
      value: '',
      message: '',
      rules: rules.email,
      state: { ...state },
      isVisible: true,
    },
}};

const initialState = {
  isSubmited: false,
  form: newForm(),
};

initialState.form = initMessages(initialState.form);

const reducer = formPropertyName => (state, action) => {
  switch (action.type) {
    case 'server-error':
      return {
        ...state,
        form: {
          email: { ...state.form.email, message: 'Adresse email non trouvée' },
        },
      };
      break;
    default:
      return formReducer(formPropertyName)(state, action);
  }
};

const submit = (form, dispatch) => {
  if (!isFormValid(form)) {
    return;
  }
  const dataToSend = {
    email: form.email.value,
  };

  return $http
    .post(master.getUrl('Account/ResetPassword'), dataToSend)
      .then(function(response) {
        const result = response.data;
        if (result.isSuccess) {
        toastr.success(
              "Demande envoyé avec succès. Un email à été envoyé à l'adresse indiquée. Suivez les étapes indiquées dans le mail.",
              'Confirmation utilisateur'
        );
        } else {
          dispatch("server-error");
        }
        return response.data;
      });
};

const ResetPassword = () => {
  useEffect(() => {
    page.setTitle('Demande ré-initialisation mot de passe');
  });
  const [state, dispatch] = useReducer(reducer('form'), initialState);
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
    submit(state.form, dispatch);
  };
  const events = { onBlur, onChange, onFocus };
  const status = getMessageStatus(state.form, state.isSubmited);
  return (
    <div className="row">
        <div className="col-md-10 col-md-offset-1 col-xs-12 col-xs-offset-0">
        <h1>Demande ré-initialisation mot de passe</h1>
          <h2>Votre email</h2>
        <form
          role="form"
          name="form"
          className="form-horizontal"
          encType="multipart/form-data"
          noValidate
                onSubmit={onSubmit}>

            <div className={"form-group form-group-lg "+ status.email.className} >
              <label htmlFor="Email" className="col-sm-3 control-label">Email: </label>
              <div className="col-sm-4">
              <input
                id="Email"
                type="email"
                name="email"
                value={state.form.email.value}
                {...events}
                className="form-control"
              />
                <span className="error-block">{status.email.message}</span>
            </div>
            </div>
            <div>
            <hr />
            <div className="form-group form-group-lg">
                <div className="col-sm-offset-3 col-sm-9">
                  <button type="submit" className="btn btn-lg btn-warning"><span
                  <span className="glyphicon glyphicon-ok"></span> Demander
                </button>
                </div>
            </div>
            </div>
        </form>
      </div>
      </div>
  );
};

app.component(name, react2angular(ResetPassword, []));

export default name;

