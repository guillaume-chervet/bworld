import history from '../../history';
import { toast as toastr } from '../../shared/services/toastr-factory';
import { userNotification } from '../../user/info/userNotification-factory';
import { master } from '../../shared/providers/master-provider';
import { breadcrumb } from '../../breadcrumb/breadcrumb-factory';
import $http from '../../http';
import $route from '../../route';

const data = {
  chat: {
    messages: [],
    to: [],
  },
};

const getModuleId = function() {
  return master.getModuleId();
};

const display = function(objData) {
  if (objData) {
    return objData;
  }
  return 'Non renseigné';
};

const getMessage = function(message, isSummary) {
  let messageResult = '';
  try {
    var msgObj = JSON.parse(message.message);
    if (message.messageType === 'SiteNotAuthenticated') {
      if (isSummary) {
        messageResult = msgObj.message;
      } else {
        const lineJump = '\n';
        let msg = 'Nom: ' + msgObj.lastName + ' ' + msgObj.firstName + lineJump;
        msg += 'Email: ' + msgObj.email + lineJump;
        msg += 'Téléphone: ' + display(msgObj.phone) + lineJump;
        msg += 'Message: ' + msgObj.message;

        messageResult = msg;
      }
    } else {
      messageResult = msgObj.message;
    }
  } catch (e) {
    console.log('Message is not a json valid');
    return message.message;
  }
  if (isSummary && messageResult && messageResult.length > 20) {
    return messageResult.substring(0, 20) + '...';
  }
  return messageResult;
};

const mapMessages = function(response) {
  if (response && response.data.data.chat) {
    const chat = data.chat;
    const chatResponse = response.data.data.chat;

    chat.title = chatResponse.title;
    chat.to.length = 0;
    chat.to.push(...chatResponse.to);
    chat.createdDate = chatResponse.createdDate;
    chat.lastMessageDate = chatResponse.lastMessageDate;
    chat.readed = chatResponse.readed;
    chat.id = chatResponse.id;
    chat.messages.length = 0;
    var messages = chat.messages;
    var messagesResponse = chatResponse.messages;
    messagesResponse.forEach(function(message) {
      messages.push({
        createdDate: message.createdDate,
        from: message.from,
        message: getMessage(message),
      });
    });
  }
};

const initAsync = function(isAdmin, userId) {
  const siteId = master.site.siteId;
  let chatId = null;
  const current = $route.current();
  if (current) {
    chatId = current.params.chatId;
  }
  let apiUrl = '';
  if (isAdmin) {
    apiUrl = 'api/contact/message/0/' + siteId + '/' + chatId;
  } else {
    apiUrl = 'api/contact/message/1/' + userId + '/' + chatId;
  }

  return $http.get(master.getUrl(apiUrl)).then(function(response) {
    if (response && response.data.data.chat) {
      userNotification.initAsync();
      mapMessages(response);
      const search = history.search();
      const f = search.f;
      const items = [];
      if (isAdmin) {
        items.push({
          url: '/administration',
          title: 'Administration',
          active: false,
          module: 'Administration',
        });
      } else {
        items.push({
          url: '/utilisateur',
          title: 'Utilisateur',
          active: false,
          module: 'User',
        });
      }
      let urlMessage = '';
      if (isAdmin) {
        urlMessage = '/administration/messages';
      } else {
        urlMessage = 'utilisateur/messages';
      }
      if (f) {
        urlMessage += '?f=' + encodeURIComponent(f);
      }
      items.push({
        url: urlMessage,
        title: 'Messages',
        active: false,
        module: 'Free',
      });
      let messageUrl = '';
      if (isAdmin) {
        messageUrl = '/administration/messages/message/' + chatId;
      } else {
        messageUrl = '/utilisateur/messages/message/' + chatId;
      }
      items.push({
        url: messageUrl,
        title: 'Message',
        active: true,
      });
      breadcrumb.setItems(items);
    }
  });
};

const sendMessageAsync = function(data) {
  const promise = $http
    .post(master.getUrl('api/contact/message'), data, {
      headers: {
        loaderMessage: 'Envoi en cours...',
      },
    })
    .then(function(response) {
      toastr.success('Message envoyé avec succès.', 'Envoi message');
      mapMessages(response);
    });
  return promise;
};

export const message = {
  initAsync,
  sendMessageAsync,
  getModuleId,
  getMessage,
  data,
};
