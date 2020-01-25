import history from '../../history';
import { master } from '../../shared/providers/master-provider';
import { message } from './message-factory';
import $http from '../../http';

const data = {
  messages: [],
};

const getModuleId = function() {
  return master.getModuleId();
};

const initAsync = function(isAdmin, userId) {
  const siteId = master.site.siteId;
  const search = history.search();
  const f = search.f;
  let urlApi = '';
  let urlBase = '';
  if (isAdmin) {
    urlApi = `api/contact/messages/0/${siteId}`;
    urlBase = '/administration';
  } else {
    urlApi = `api/contact/messages/1/${userId}`;
    urlBase = '/utilisateur';
  }
  if (f) {
    urlApi += `?f=${encodeURIComponent(f)}`;
  }
  data.messages.length = 0;
  return $http.get(master.getUrl(urlApi)).then(function(response) {
    if (response) {
      let postUrl = '';
      if (f) {
        postUrl += `?f=${f}`;
      }
      const result = response.data.data;
      result.chats.forEach(function(chat) {
        const lastMessage = chat.messages[chat.messages.length - 1];
        const from = lastMessage.from;
        const msg = message.getMessage(lastMessage, true);
        const to = [];
        chat.to.forEach(function(t) {
          if (t.id !== from.id) {
            to.push(t);
          }
        }, this);

        data.messages.push({
          title: chat.title,
          readed: chat.readed,
          message: msg,
          from: lastMessage.from,
          to: to,
          date: lastMessage.createdDate,
          url: `${urlBase}/messages/message/${chat.id}${postUrl}`,
        });
      });
      const filterNext = JSON.stringify({
        date: result.dateNext,
        isPrevious: false,
      });
      const filterPrevious = JSON.stringify({
        date: result.datePrevious,
        isPrevious: true,
      });

      const urlNext = `${urlBase}/messages/?f=${filterNext}`;
      const urlPrevious = `${urlBase}/messages/?f=${filterPrevious}`;
      data.urlNext = urlNext;
      data.numberNext = result.numberNext;
      data.urlPrevious = urlPrevious;
      data.numberPrevious = result.numberPrevious;
    }
  });
};

export const messages = {
  initAsync: initAsync,
  getModuleId: getModuleId,
  data: data,
};
