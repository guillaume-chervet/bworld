import _ from 'lodash';
import React, { useEffect, useReducer } from 'react';
import { validation } from 'mw.validation';

import './message.css';

const rules = {
  email: ['required', 'email'],
  lastName: ['required', 'lastName'],
  title: ['required'],
  message: ['required'],
  firstName: ['required', 'firstName'],
  phone: ['phone'],
};

export const state = {
  hasChange: false,
  hasFocus: false,
  hasLostFocusOnce: false,
  messageCapture: '',
};

const newMessage = () => {
  return {
    title: {
      value: '',
      message: '',
      rules: rules.title,
      state: { ...state },
      isVisible: true,
    },
    lastName: {
      value: '',
      message: '',
      rules: rules.lastName,
      state: { ...state },
      isVisible: true,
    },
    firstName: {
      value: '',
      message: '',
      rules: rules.firstName,
      state: { ...state },
      isVisible: true,
    },
    email: {
      value: '',
      message: '',
      rules: rules.email,
      state: { ...state },
      isVisible: true,
    },
    phone: {
      value: '',
      message: '',
      rules: rules.phone,
      state: { ...state },
      isVisible: true,
    },
    message: {
      value: '',
      message: '',
      rules: rules.message,
      state: { ...state },
      isVisible: true,
    },
  };
};

const initialState = {
  count: 0,
  messageSended: false,
  isSubmited: false,
  message: newMessage(),
};

export const validateInput = (input, value) => {
  const validationResults = validation.validateView(value, input.rules);

  const firstFailed = validationResults.find(function(element) {
    return !element.success;
  });

  return firstFailed ? firstFailed.message : '';
};

export const initMessages = form => {
  const newForm = {};
  for (let [key, value] of Object.entries(form)) {
    const input = form[key];
    newForm[key] = {
      ...input,
      value: '',
      message: validateInput(input, input.value),
    };
  }
  return newForm;
};

initialState.message = initMessages(initialState.message);

export const formReducer = formPropertyName => (state, action) => {
  switch (action.type) {
    case 'onSubmit':
      return { ...state, isSubmited: true };
    case 'onChange': {
      const target = action.data;
      const name = target.name;
      const value = target.value;
      const newForm = { ...state[formPropertyName] };
      const input = state[formPropertyName][name];
      const message = validateInput(input, value);
      newForm[name] = {
        ...input,
        value,
        message: message,
        state: { ...input.state, hasChange: true },
      };
      return { ...state, [formPropertyName]: newForm };
    }
    case 'onFocus': {
      const target = action.data;
      const name = target.name;
      const newForm = { ...state[formPropertyName] };
      const input = state[formPropertyName][name];
      newForm[name] = {
        ...input,
        state: {
          ...input.state,
          hasFocus: true,
          messageCapture: input.message,
        },
      };
      return { ...state, [formPropertyName]: newForm };
    }
    case 'onBlur': {
      const target = action.data;
      const name = target.name;
      const newForm = { ...state[formPropertyName] };
      const input = state[formPropertyName][name];
      newForm[name] = {
        ...input,
        state: { ...input.state, hasLostFocusOnce: true, hasFocus: false },
      };
      return { ...state, [formPropertyName]: newForm };
    }

    default:
      return new Error();
  }
};

const reducer = (state, action) => {
  switch (action.type) {
    case 'onInit':
      if (action.isAuthenticate) {
        return {
          ...state,
          message: {
            ...state.message,
            lastName: { ...state.message.lastName, isVisible: false },
            firstName: { ...state.message.firstName, isVisible: false },
            email: { ...state.message.email, isVisible: false },
            phone: { ...state.message.phone, isVisible: false },
          },
        };
      } else {
        return {
          ...state,
          message: {
            ...state.message,
            lastName: { ...state.message.lastName, isVisible: true },
            firstName: { ...state.message.firstName, isVisible: true },
            email: { ...state.message.email, isVisible: true },
            phone: { ...state.message.phone, isVisible: true },
          },
        };
      }
    case 'initMessage':
      return {
        ...state,
        message: initMessages(state.message),
        messageSended: false,
        isSubmited: false,
      };
    case 'messageSended':
      return { ...state, messageSended: true };
    default:
      return formReducer('message')(state, action);
  }
};

export const getMessage = (input, forceDisplayMessage) => {
  const message = input.message;
  const { hasChange, hasLostFocusOnce, hasFocus, messageCapture } = input.state;
  const isDisplayMessage =
    hasLostFocusOnce || forceDisplayMessage || (hasChange && !hasFocus);
  if (!isDisplayMessage) {
    return '';
  }
  if (hasFocus) {
    return messageCapture;
  }
  return message;
};

export const isFormValid = formMessage => {
  for (let [key, value] of Object.entries(formMessage)) {
    if (formMessage[key].isVisible && formMessage[key].message) {
      return false;
    }
  }

  return true;
};

export const MessageContainer = React.memo(({
  element,
  moduleId,
  siteId,
  user,
  sendMessageAsync,
}) => {
  const [state, dispatch] = useReducer(reducer, initialState);
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
    submit(state.message, user, siteId, moduleId, sendMessageAsync);
  };

  const onInit = () => {
    const { isAuthenticate } = user;
    dispatch({ type: 'onInit', isAuthenticate });
  };

  useEffect(() => {
    onInit();
  }, []);

  const initMessage = () => dispatch({ type: 'initMessage' });

  const submit = (formMessage, user, siteId, moduleId, sendMessageAsync) => {
    const messageData = {};
    for (let [key, value] of Object.entries(formMessage)) {
      const input = formMessage[key];
      messageData[key] = input.value;
    }

    if (!isFormValid(formMessage)) {
      return;
    }
    let messageToSend = null;
    let type = '';
    const from = {
      id: null,
      type: 1,
    };
    if (user.isAuthenticate) {
      from.id = user.id;
      messageToSend = {
        message: messageData.message,
        title: messageData.title,
        moduleId: moduleId,
      };
      type = 'SiteAuthenticated';
    } else {
      from.id = messageData.email;
      from.type = 2;
      messageToSend = {
        moduleId,
      };
      Object.assign(messageToSend, _.cloneDeep(messageData));
      type = 'SiteNotAuthenticated';
    }

    const data = {
      from: from,
      to: {
        id: siteId,
        type: 0,
      },
      type: type,
      source: 'User',
      messageJson: JSON.stringify(messageToSend),
    };

    sendMessageAsync(data).then(function() {
      dispatch({ type: 'messageSended' });
    });
  };

  return (
    <MessageMemo
      user={user}
      onBlur={onBlur}
      onFocus={onFocus}
      onChange={onChange}
      onSubmit={onSubmit}
      message={state.message}
      initMessage={initMessage}
      isSubmited={state.isSubmited}
      messageSended={state.messageSended}
      element={element}></MessageMemo>
  );
});

const getClassLabel = element => {
  if (element.$level <= 2) {
    return 'col-sm-3';
  }
  return 'col-sm-12 col-md-4';
};
const getClassField = element => {
  if (element.$level <= 2) {
    return 'col-sm-6';
  }
  return 'col-sm-12 col-md-8';
};
const getClassAction = element => {
  if (element.$level <= 2) {
    return 'col-sm-offset-3 col-sm-9 col-xs-offset-6 col-xs-6 mw-action';
  }
  return 'col-sm-offset-4 col-sm-9 col-md-offset-4 col-md-8 col-xs-offset-6 col-xs-6 mw-action';
};

export const getMessageStatus = (message, isSubmited) => {
  const status = {};
  for (let [key, value] of Object.entries(message)) {
    const input = message[key];
    const errorMessage = getMessage(input, isSubmited);
    status[key] = {
      message: errorMessage,
      className:
        'form-group form-group-lg ' +
        (errorMessage ? 'has-error has-feedback' : ''),
    };
  }
  return status;
};



const Message = ({
  element,
  message,
  messageSended,
  onChange,
  onSubmit,
  onFocus,
  onBlur,
  initMessage,
  isSubmited,
}) => {
  const status = getMessageStatus(message, isSubmited);

  const events = { onBlur, onChange, onFocus };

  return (
    <div className="mw-message-element">
      {!messageSended && (
        <div>
          <form
            role="form"
            name="formMessage"
            className="form-horizontal"
            noValidate
            encType="multipart/form-data"
            onSubmit={onSubmit}>
            <fieldset>
              <legend>Envoyer un message</legend>

              <div className={status.title.className}>
                <label
                  htmlFor="Title"
                  className={`${getClassLabel(element)} control-label`}>
                  Titre*
                </label>
                <div className={getClassField(element)}>
                  {element.data.subjects.length > 0 ? (
                    <select
                      id="Title"
                      name="title"
                      className="form-control"
                      value={message.title.value}
                      onChange={onChange}>
                      <option value="">- Sélectionner -</option>
                      {element.data.subjects.map(c => (
                        <option value={c.title}>{c.title}</option>
                      ))}
                    </select>
                  ) : (
                    <input
                      type="text"
                      id="Title"
                      name="title"
                      value={message.title.value}
                      className="form-control"
                      {...events}
                    />
                  )}
                  <span className="error-block">{status.title.message}</span>
                </div>
              </div>

              {message.lastName.isVisible && (
                <div className={status.lastName.className}>
                  <label
                    htmlFor="LastName"
                    className={`${getClassLabel(element)} control-label`}>
                    Nom*
                  </label>
                  <div className={getClassField(element)}>
                    <input
                      type="text"
                      name="lastName"
                      id="LastName"
                      value={message.lastName.value}
                      className="form-control"
                      {...events}
                    />
                    <span className="error-block">
                      {status.lastName.message}
                    </span>
                  </div>
                </div>
              )}

              {message.firstName.isVisible && (
                <div className={status.firstName.className}>
                  <label
                    htmlFor="FirstName"
                    className={`${getClassLabel(element)} control-label`}>
                    Prénom*
                  </label>
                  <div className={getClassField(element)}>
                    <input
                      type="text"
                      name="firstName"
                      id="FirstName"
                      value={message.firstName.value}
                      className="form-control"
                      {...events}
                    />
                    <span className="error-block">
                      {status.firstName.message}
                    </span>
                  </div>
                </div>
              )}

              {message.email.isVisible && (
                <div className={status.email.className}>
                  <label
                    htmlFor="Email"
                    className={`${getClassLabel(element)} control-label`}>
                    Email*
                  </label>
                  <div className={getClassField(element)}>
                    <input
                      type="text"
                      name="email"
                      id="Email"
                      value={message.email.value}
                      className="form-control"
                      {...events}
                    />
                    <span className="error-block">{status.email.message}</span>
                  </div>
                </div>
              )}

              {message.phone.isVisible && (
                <div className={status.phone.className}>
                  <label
                    htmlFor="Phone"
                    className={`${getClassLabel(element)} control-label`}>
                    Téléphone
                  </label>
                  <div className={getClassField(element)}>
                    <input
                      type="text"
                      name="phone"
                      id="Phone"
                      value={message.phone.value}
                      className="form-control"
                      {...events}
                    />
                    <span className="error-block">{status.phone.message}</span>
                  </div>
                </div>
              )}

              <div className={status.message.className}>
                <label
                  htmlFor="uMessage"
                  className={`${getClassLabel(element)} control-label`}>
                  Message*
                </label>
                <div className={getClassField(element)}>
                  <textarea
                    value={message.message.value}
                    {...events}
                    className="form-control"
                    name="message"
                    rows="6"
                    cols="30"
                  />
                  <span className="error-block">{status.message.message}</span>
                </div>
              </div>
              <div className="form-group form-group-lg">
                <div className={getClassAction(element)}>
                  <button type="submit" className="btn btn-lg btn-success">
                    <span className="glyphicon glyphicon-send" /> Envoyer
                  </button>
                </div>
              </div>
            </fieldset>
          </form>
        </div>
      )}

      {messageSended && (
        <div>
          <h2>Message envoyé</h2>
          <p>Votre message a été envoyé avec succès</p>
          <div className="text-center">
            <button
              type="button"
              className="btn btn-primary btn-lg"
              onClick={initMessage}>
              Nouveau message
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

const MessageMemo = React.memo(Message);
