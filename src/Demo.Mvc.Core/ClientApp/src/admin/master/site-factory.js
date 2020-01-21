import { master } from '../../shared/providers/master-provider';
import { free } from '../../free/free-factory';
import { toast as toastr } from '../../shared/services/toastr-factory';
import { service as elementFileService } from '../../elements/file/elementFile-factory';
import $http from '../../http';
import $window from '../../window';
import redux from '../../redux';

const elements = [
  {
    type: 'h1',
    property: 'Title',
    label: 'Titre principal',
    data: '',
    help:
      'Titre qui apparaît dans chaque page, chaque email ainsi que dans les moteurs de recherche.',
  },
  {
    type: 'file',
    property: 'ImageLogo',
    data: [],
    label: 'Logo',
    config: {
      maxWidth: 1024,
      maxHeigth: 420,
    },
  }, //  (800*400 pixel max)
  {
    type: 'checkbox',
    property: 'Jumbotron',
    label: 'Jumbotron',
    data: false,
    placeholder: ' Les logos utilisent la totalité de la largeur de la page.',
  },
  {
    type: 'file',
    property: 'ImageIcone',
    label: 'Icône',
    data: [],
    config: {
      width: 64,
      heigth: 64,
      typeMime: 'image/png',
    },
  },
  {
    type: 'color',
    property: 'ColorBackgroundMenu',
    label: 'Couleur du menu',
    data: '',
  },
  {
    type: 'color',
    property: 'ColorBackgroundMenuBottom',
    label: 'Couleur du menu du bas',
    data: '',
  },
  {
    type: 'color',
    property: 'ColorJumbotron',
    label: "Couleur sous l'image principale",
    data: '',
  },
  {
    type: 'color',
    property: 'ColorH1',
    label: 'Couleur titres principaux',
    data: '',
  },
  {
    type: 'color',
    property: 'ColorH2',
    label: 'Couleur titres secondaires',
    data: '',
  },
  {
    type: 'color',
    property: 'Color',
    label: 'Couleur du text',
    data: '',
  },
  {
    type: 'color',
    property: 'ColorBackground',
    label: "Couleur d'arrière plan de la page",
    data: '',
  },
  {
    type: 'select',
    property: 'Theme',
    label: 'Theme',
    data: 'default',
  },
];

const mapFileData = elementFileService.mapFileData;

const getThemeValue = function(elements) {
  var themeElement = free.getFirstElement(elements, ['select'], null);
  if (themeElement) {
    return themeElement.data;
  }
  return null;
};

let _themeValue = null;
const initAsync = function() {
  const siteId = master.site.siteId;
  return $http
    .get(master.getUrl('api/site/get/' + siteId))
    .then(function(response) {
      if (response) {
        const state = redux.getState();
        const masterId = state.master.masterServer.id;
        response.data.data.elements.forEach(element => {
          if (element.type === 'image' || element.type === 'file') {
            const fileElement = free.getElement(elements, element.property);
            const fileDatas = JSON.parse(element.data);

            fileElement.data.length = 0;

            fileDatas.forEach(serveurElement => {
              const fileData = mapFileData(
                serveurElement,
                masterId,
                master.site.siteId
              );
              fileElement.data.push(fileData);
            }, this);
          } else if (element.type === 'checkbox') {
            const checkElement = free.getElement(elements, element.property);
            checkElement.data = element.data === 'true';
          } else {
            const otherElement = free.getElement(elements, element.property);
            if (otherElement) {
              otherElement.data = element.data;
            }
          }
        });
        _themeValue = getThemeValue(elements);
      }
    });
};

function saveAsync() {
  const elementsTemp = free.mapElement(elements);
  const state = redux.getState();
  const masterId = state.master.masterServer.master.id;
  const dataToSend = {
    site: state.master.masterServer.site,
    moduleId: masterId,
    elements: elementsTemp,
  };
  return $http
    .post(master.getUrl('api/site/save'), dataToSend)
    .then(function(response) {
      if (response.data.isSuccess) {
        master.updateMaster(response.data.data);
      }
      toastr.success('Sauvegarde effectuée avec succès.', 'Sauvegarde site');
      const themeValue = getThemeValue(elements);
      if (_themeValue !== themeValue) {
        $window.location.reload(true);
      }
    });
}

export const site = {
  initAsync: initAsync,
  saveAsync: saveAsync,
  elements: elements,
};
