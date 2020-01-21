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

const checkAsync = dataToSend => {
  localStorage.put('site', site);
  return $http
    .post(master.getUrl('api/site/check'), dataToSend)
    .then(function(response) {
      return response.data;
    });
};

const saveAsync = dataToSend => {
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
};

const saveAdminAsync = data => {
  const elementsTemp = free.mapElement(
    pageData.elements,
    pageData.metaElements
  );
  const moduleId = master.getModuleId();
  const dataToSend = {
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
};

const initMenuAdmin = (menuItems, menuItem) => {
  menuItems.push({
    routePath: master.getInternalPath(
      '/administration/' + menuItem.routePathWithoutHomePage
    ),
    title: menuItem.title,
    module: 'AddSite',
    icon: menuItem.icon,
    moduleId: menuItem.routeDatas.moduleId,
  });
};

const getPaths = () => {
  const moduleId = master.getModuleId();
  const menutItem = master.getServerMenuItem(moduleId);
  const validation = menutItem.routePathWithoutHomePage + '/validation';
  const authentification =
    menutItem.routePathWithoutHomePage + '/authentification';
  const configuration = menutItem.routePathWithoutHomePage + '/configuration';

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

const getCurrentPath = () => {
  const path = history.path();
  const paths = getPaths();

  for (let key in paths) {
    if (path.indexOf(paths[key].path) >= 0) {
      return key;
    }
  }
};

const navNext = function() {
  var currentPath = getCurrentPath();
  var paths = getPaths();
  if (currentPath === 'configuration') {
    history.search({ dm: false }, paths.validation.path);
  } else if (currentPath === 'authentification') {
    history.search({ dm: false }, paths.configuration.path);
  } else {
    history.search({ dm: false }, paths.validation.path);
  }
};

const navBack = () => {
  const currentPath = getCurrentPath();
  const paths = getPaths();
  if (currentPath === 'validation') {
    history.search({ dm: false }, paths.configuration.path);
  } else if (currentPath === 'configuration') {
    history.search({ dm: false }, paths.authentification.path);
  } else {
    var menuItem = master.getCurrentMenuItem();
    history.search({ dm: null }, menuItem.routePath);
  }
};

const navStart = () => {
  localStorage.remove('site');
  navNext();
};

const getDomain = function() {
  if (!site.domain) {
    return in18Util.display('');
  }
  const domainToParse = site.domain;
  const domain = urlHelper.normaliseUrl(domainToParse);
  return in18Util.display(`https://www.${domain}.fr`);
};

const getDomainTemporary = () => {
  if (!site.domain) {
    return in18Util.display('');
  }
  let domainToParse = site.domain;
  if (site.category) {
    for (let i = 0; i < data.templates.length; i++) {
      const template = data.templates[i];
      if (site.category === template.categoryId) {
        domainToParse = `${template.title}-${domainToParse}`;
        break;
      }
    }
  }
  const domain = urlHelper.normaliseUrl(domainToParse);
  if (!domain) {
    return in18Util.display('');
  } else {
    const fullAddress = `https://${domain}.bworld.fr`;
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
