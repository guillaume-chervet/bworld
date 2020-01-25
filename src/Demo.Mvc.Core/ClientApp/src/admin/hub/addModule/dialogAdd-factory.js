import modal from '../../../modal';
import modulesFactory from '../../../modules-factory';
import { master } from '../../../shared/providers/master-provider';
import { getIconBase } from '../../../shared/icons';
import _ from 'lodash';

const modes = { Default: 'Default', Private: 'private' };

const modules = [
  {
    id: '1',
    label: 'Page libre',
    name: 'Free',
    module: 'Free',
    modes: [modes.Default, modes.Private],
    icon: getIconBase('free'),
    description: "Ajout d'une page éditable très simplement",
  },
  {
    id: '2',
    label: 'Page contact',
    name: 'Contact',
    module: 'Free',
    modes: [modes.Default, modes.Private],
    icon: getIconBase('contact'),
    description: "Ajout d'une page de contact",
  },
  {
    id: '3',
    label: 'Page article',
    name: 'News',
    module: 'News',
    modes: [modes.Default, modes.Private],
    icon: getIconBase('news'),
    description: "Ajout d'une page d'article",
  },
  {
    id: '4',
    label: 'Lien social',
    name: 'Social',
    module: 'Social',
    modes: [modes.Default],
    icon: getIconBase('social'),
    description: "Ajout d'un lien (facebook, n° téléphone, email, etc.)",
  },
];

const openAsync = function(propertyName, mode) {
  if (!mode) {
    mode = modes.Default;
  }
  const modulesSelected = _.filter(modules, function(o) {
    return o.modes.indexOf(mode) > -1;
  });

  const newModules = modulesSelected.map(module => {
    const newModule = { ...module };
    newModule.iconUrl = getImageUrl(module.name);
    return newModule;
  });

  const modalInstance = modal.open({
    template:
      '<dialog-add close="$close()" dismiss="$dismiss()" data="$ctrl.data"></dialog-add>',
    size: 'lg',
    controller: function() {
      this.data = {
        modules: newModules,
        propertyName: propertyName,
        mode: mode,
      };
      return this;
    },
    controllerAs: '$ctrl',
    resolve: {
      element: function() {
        return null;
      },
    },
  });

  return modalInstance;
};

const addAsync = function(module, propertyName) {
  const service = modulesFactory.getModule(module.module);
  if (!propertyName) {
    propertyName = 'MenuItems';
  }
  if (service && service.service && service.service.initMenuAdmin) {
    const promise = service.service.addAsync(propertyName, module.icon);
    return promise;
  }
  return null;
};

const getImageUrl = moduleName => {
  if (!moduleName) {
    return '';
  }
  const service = modulesFactory.getModule(moduleName);
  if (service && service.iconUrl) {
    return service.iconUrl;
  }

  // const path = await import(`../../../${moduleName}/icon.png`);
  return ''; //path.default;
};

export const dialogAdd = {
  openAsync: openAsync,
  addAsync: addAsync,
  modules: modules,
};
