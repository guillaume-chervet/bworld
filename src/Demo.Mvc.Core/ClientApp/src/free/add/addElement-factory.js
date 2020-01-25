import modal from '../../modal';
import { master } from '../../shared/providers/master-provider';

const modules = [
  {
    index: 0,
    label: 'Text',
    name: 'p',
    icon: 'fa fa-bars',
    description: 'Text de contenu.',
    parents: ['div', 'gridElement'],
    modes: ['default', 'mail'],
  },
  {
    index: 0,
    label: 'Média',
    name: 'file',
    icon: 'fa fa-camera',
    description: 'Liste de média de type images, vidéos, etc.',
    parents: ['div', 'gridElement'],
    modes: ['default', 'mail'],
  },
  {
    index: 0,
    label: 'Carousel',
    name: 'carousel',
    icon: 'fa fa-camera',
    description: 'Images qui défilent',
    parents: ['div', 'gridElement'],
    modes: ['default'],
  },
  {
    index: 0,
    label: 'Séparateur',
    name: 'hr',
    icon: 'glyphicon glyphicon-minus',
    description: "Séparateur d'élement",
    parents: ['div', 'gridElement'],
    modes: ['default', 'mail'],
  },
  {
    index: 1,
    label: 'Adresse + Carte',
    name: 'address',
    icon: 'fa fa-map',
    description: 'Adresse + Carte qui affiche la position',
    parents: ['div', 'gridElement'],
    modes: ['default'],
  },
  {
    index: 1,
    label: 'Message',
    name: 'message',
    icon: 'fa file-code-o',
    description: "Formulaire d'envoie de message",
    parents: ['div', 'gridElement'],
    modes: ['default'],
  },
  {
    index: 3,
    label: 'Code',
    name: 'code',
    icon: 'fa file-code-o',
    description: 'Code informatique',
    parents: ['div'],
    modes: ['default'],
  },
  {
    index: 2,
    label: 'Youtube',
    name: 'youtube',
    icon: 'glyphicon glyphicon-minus',
    description: 'Vidéos youtube',
    parents: ['div', 'gridElement'],
    modes: ['default'],
  },
  {
    index: 1,
    label: 'Horaires',
    name: 'hours',
    icon: 'glyphicon glyphicon-minus',
    description: "Horaires d'ouverture",
    parents: ['div', 'gridElement'],
    modes: ['default'],
  },
];

modules.push({
  index: 0,
  label: 'GridElement',
  name: 'gridElement',
  icon: 'fa file-code-o',
  description: 'Colonne responsive de la grille',
  parents: ['grid'],
  modes: ['default'],
});
modules.push({
  index: 0,
  label: 'Grid',
  name: 'grid',
  icon: 'fa file-code-o',
  description: "Permet de réaliser une Grille d'élément",
  parents: ['div'],
  modes: ['default'],
});

modules.push({
  index: 0,
  label: 'Link',
  name: 'link',
  icon: 'fa file-code-o',
  description: "Permet d'insèrer un lien vers une autre page de votre site",
  parents: ['div', 'gridElement'],
  modes: ['default', 'mail'],
});

modules.push({
  index: 3,
  label: 'Formulaire',
  name: 'form',
  icon: 'fa file-code-o',
  description: "Permet d'insèrer un formulaire",
  parents: ['div'],
  modes: ['default'],
});

modules.push({
  index: 3,
  label: 'Question simple/multiple',
  name: 'inputRadio',
  icon: 'fa file-code-o',
  description: "Permet d'insèrer une question à choix simple ou mutliple",
  parents: ['form>div'],
  modes: ['default'],
});

const openAsync = function(element, mode) {
  const listModules = [
    {
      index: 0,
      modules: [],
      title: 'Basique',
    },
    {
      index: 1,
      modules: [],
      title: 'Fonctionnel',
    },
    {
      index: 2,
      modules: [],
      title: 'Social',
    },
    {
      index: 3,
      modules: [],
      title: 'Technique',
    },
  ];
  if (!mode) {
    mode = 'default';
  }

  for (var j = 0; j < listModules.length; j++) {
    modules.forEach(function(module) {
      getImageUrlAsync(module.name).then(url => (module.iconUrl = url));
      let parent = element.$parent;
      if (!parent) {
        parent = element;
      }
      // TODO remove pas propre :p
      if (element.type === 'grid') {
        parent = element;
      }
      if (parent) {
        let isParent = module.parents.indexOf(parent.type) > -1;
        if (!isParent && parent.$parent) {
          isParent =
            module.parents.indexOf(`${parent.$parent.type}>${parent.type}`) >
            -1;
        }

        const indexMode = module.modes.indexOf(mode);
        if (indexMode > -1 && isParent && module.index === j) {
          const data = listModules[j];
          data.modules.push(module);
        }
      }
    });
  }

  const filteredModules = listModules.filter(
    currentObject => currentObject.modules.length > 0
  );

  const modalInstance = modal.open({
    animation: true,
    component: 'dialogAddElement',
    size: 'lg',
    resolve: {
      item: function() {
        return {
          listModules: filteredModules,
        };
      },
    },
  });

  return modalInstance;
};

const getImageUrlAsync = async moduleName => {
  if (!moduleName) {
    return '';
  }
  const path = await import(`../../elements/${moduleName}/icon.png`);
  return path.default;
};

export const addElement = {
  openAsync: openAsync,
  modules: modules,
};
