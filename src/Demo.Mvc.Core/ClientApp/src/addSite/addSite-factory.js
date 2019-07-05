import _ from 'lodash';

import { master } from '../shared/providers/master-provider';
import { in18Util } from '../shared/services/in18Util-factory';
import { urlHelper } from '../shared/services/urlHelper-factory';
import { localStorage } from '../shared/services/localStorage-factory';
import { toast as toastr } from '../shared/services/toastr-factory';

import { free } from '../free/free-factory';

import $http from '../http';
import history from '../history';
import window from '../window';

const data = {};
const pageData = free.createData();
let site = localStorage.get('site');
if (!site) {
  site = {};
}
const initAsync = function(menuKey) {
  const moduleId = master.getModuleId(null, menuKey);
  const siteId = master.site.siteId;
  return $http
    .get(master.getUrl('api/site/load/' + siteId + '/' + moduleId))
    .then(function(response) {
      if (response) {
        const result = response.data.data;
        free.initData(result, pageData);
        Object.assign(data, _.cloneDeep(result));
      }
    });
};

function checkAsync(dataToSend) {
  localStorage.put('site', site);
  return $http
    .post(master.getUrl('api/site/check'), dataToSend)
    .then(function(response) {
      return response.data;
    });
}

function saveAsync(dataToSend) {
  return $http
    .post(master.getUrl('api/site/add'), dataToSend, {
      headers: {
        loaderMessage: 'Création du site en cours...',
      },
    })
    .then(function(response) {
      if (response.data.isSuccess) {
        localStorage.remove('site');
        toastr.success(
          'Création du site réalisé avec succès. Vous allez être redirigé vers votre nouveau site.',
          'Création du site'
        );
        window.location = response.data.data;
      }
      return response.data;
    });
}

function saveAdminAsync(data) {
  var elementsTemp = free.mapElement(pageData.elements, pageData.metaElements);
  var moduleId = master.getModuleId();
  var dataToSend = {
    templates: data.templates,
    moduleId: moduleId,
    urlConditionsGeneralesUtilisations: data.urlConditionsGeneralesUtilisations,
    elements: elementsTemp,
    site: master.site,
  };

  return $http
    .post(master.getUrl('api/site/saveaddsite'), dataToSend)
    .then(function(response) {
      toastr.success(
        'Sauvegarde effectuée avec succès.',
        'Sauvegarde page création sites'
      );
      if (response.data.isSuccess) {
        master.updateMaster(response.data.data.master);
      }
      return response.data;
    });
}

var initMenuAdmin = function(menuItems, menuItem) {
  menuItems.push({
      routePath: master.getInternalPath('/administration/' + menuItem.routePathWithoutHomePage),
    title: menuItem.title,
    module: 'AddSite',
    icon: menuItem.icon,
    moduleId: menuItem.routeDatas.moduleId,
  });
};

var getPaths = function() {
  var moduleId = master.getModuleId();
  var menutItem = master.getServerMenuItem(moduleId);
  var validation = menutItem.routePathWithoutHomePage + '/validation';
  var authentification =
    menutItem.routePathWithoutHomePage + '/authentification';
  var configuration = menutItem.routePathWithoutHomePage + '/configuration';

  return {
    validation: {
      path: validation,
      url: validation + '?dm=false',
    },
    authentification: {
      path: authentification,
      url: authentification + '?dm=false',
    },
    configuration: {
      path: configuration,
      url: configuration + '?dm=false',
    },
  };
};

var getCurrentPath = function() {
  var path = history.path();
  var paths = getPaths();

  for (var key in paths) {
    if (path.indexOf(paths[key].path) >= 0) {
      return key;
    }
  }
};

var navNext = function() {
  var currentPath = getCurrentPath();
  var paths = getPaths();
  if (currentPath === 'configuration') {
    history.path(paths.validation.path);
    history.search('dm', false);
  } else if (currentPath === 'authentification') {
    history.path(paths.configuration.path);
    history.search('dm', false);
  } else {
    history.path(paths.validation.path);
    history.search('dm', false);
  }
};

var navBack = function() {
  var currentPath = getCurrentPath();
  var paths = getPaths();
  if (currentPath === 'validation') {
    history.path(paths.configuration.path);
    history.search('dm', false);
  } else if (currentPath === 'configuration') {
    history.path(paths.authentification.path);
    history.search('dm', false);
  } else {
    var menuItem = master.getCurrentMenuItem();
    history.path(menuItem.routePath);
    history.search('dm', null);
  }
};

var navStart = function() {
  localStorage.remove('site');
  navNext();
};

var getDomain = function() {
  if (!site.domain) {
    return in18Util.display('');
  }
  var domainToParse = site.domain;
  var domain = urlHelper.normaliseUrl(domainToParse);
  return in18Util.display('https://www.' + domain + '.fr');
};

var getDomainTemporary = function() {
  if (!site.domain) {
    return in18Util.display('');
  }
  var domainToParse = site.domain;
  if (site.category) {
    for (var i = 0; i < data.templates.length; i++) {
      var template = data.templates[i];
      if (site.category === template.categoryId) {
        domainToParse = template.title + '-' + domainToParse;
        break;
      }
    }
  }
  var domain = urlHelper.normaliseUrl(domainToParse);
  if (!domain) {
    return in18Util.display('');
  } else {
    var fullAddress = 'https://' + domain + '.bworld.fr';
    return in18Util.display(fullAddress);
  }
};

export const addSite = {
  initAsync: initAsync,
  saveAsync: saveAsync,
  data: data,
  pageData: pageData,
  mapParent: free.mapParent,
  initMenuAdmin: initMenuAdmin,
  saveAdminAsync: saveAdminAsync,
  checkAsync: checkAsync,
  getPaths: getPaths,
  getCurrentPath: getCurrentPath,
  navNext: navNext,
  navBack: navBack,
  navStart: navStart,
  site: site,
  getDomain: getDomain,
  getDomainTemporary: getDomainTemporary,
};
