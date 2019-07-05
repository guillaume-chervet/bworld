import modal from '../../modal';
import sce from '../../sce';
import { master } from '../../shared/providers/master-provider';
import { urlHelper } from '../../shared/services/urlHelper-factory';

const getFileExtention = function(filename) {
  return filename.substr(filename.lastIndexOf('.') + 1);
};

const mapFileData = function(serveurElement, moduleId, siteId) {
  const fileData = {};

  const type = serveurElement.Type;
  fileData.id = serveurElement.Id;
  fileData.propertyName = serveurElement.PropertyName;
  fileData.name = serveurElement.Name;
  fileData.type = type;
  fileData.description = serveurElement.Description;
  fileData.title = serveurElement.Title;
  fileData.size = serveurElement.Size;
  fileData.thumbDisplayMode = serveurElement.ThumbDisplayMode;
  fileData.behavior = serveurElement.Behavior;
  fileData.url = serveurElement.Url;
  fileData.link = {};
  if (serveurElement.Link) {
    const link = serveurElement.Link;
    fileData.link.id = link.Id;
    fileData.link.anchor = link.Anchor;
  }
  var fileName;
  const fileExtention = getFileExtention(serveurElement.Name);
  if (serveurElement.Title) {
    fileName =
      urlHelper.normaliseUrl(serveurElement.Title) + '.' + fileExtention;
  } else {
    fileName =
      urlHelper.normaliseUrl(serveurElement.Name) + '.' + fileExtention;
  }

  if (type === 'video/mp4') {
    // if (!fileData.url) {
    fileData.url =
      '/api/media/get/' + siteId + '/' + serveurElement.Id + '/' + fileName;
    //}
    fileData.displayType = 'video';
    fileData.config = {
      preload: 'none',
      sources: [{ src: fileData.url, type: 'video/mp4' }],
      tracks: [],
      theme: {
        //    url: "https://www.videogular.com/styles/themes/default/latest/videogular.css"
      },
      plugins: {
        //  poster: "https://www.videogular.com/assets/images/videogular.png"
      },
    };
  } else {
    fileData.displayType = 'image';
    if (serveurElement.Id) {
      fileData.url =
        '/api/file/get/' +
        siteId +
        '/' +
        serveurElement.Id +
        '/ImageUploaded/' +
        fileName;
      fileData.thumbnailUrl =
        '/api/file/get/' +
        siteId +
        '/' +
        serveurElement.Id +
        '/ImageThumb/' +
        fileName;
      fileData.deleteUrl =
        '/api/file/delete/' + siteId + '/' + serveurElement.Id + '/' + fileName;
    } else {
      fileData.url =
        '/api/file/get/' +
        siteId +
        '/' +
        moduleId +
        '/' +
        serveurElement.PropertyName +
        '/ImageUploaded/' +
        fileName;
      fileData.thumbnailUrl =
        '/api/file/get/' +
        siteId +
        '/' +
        moduleId +
        '/' +
        serveurElement.PropertyName +
        '/ImageThumb/' +
        fileName;
      fileData.deleteUrl =
        '/api/file/delete/' +
        siteId +
        '/' +
        moduleId +
        '/' +
        serveurElement.PropertyName +
        '/' +
        fileName;
    }
  }

  fileData.deleteType = 'DELETE';
  fileData.isTemporary = false;

  return fileData;
};

const open = function(element, file, isAdmin) {
  let template =
    '<modal-image close="$close()" dismiss="$dismiss()" data="$ctrl.data"></modal-image>';
  if (isAdmin) {
    template =
      '<modal-image-admin close="$close()" dismiss="$dismiss()" data="$ctrl.data"></modal-image-admin>';
  }
  modal.open({
    template,
    size: 'lg',
    controller: function() {
      this.data = {
        element: file,
        elements: element.data,
        parent: element.$parent,
      };
    },
    controllerAs: '$ctrl',
    resolve: {
      element: function() {
        return file;
      },
    },
  });
};

const getClass = function(file) {
  if (file && file.thumbDisplayMode) {
    return 'img-' + file.thumbDisplayMode;
  }
  return '';
};

const getAlt = function(file) {
  if (file) {
    let title = '';
    if (!file.title) {
      title = file.name;
    } else {
      title = file.title;
    }
    if (file.description) {
      return title + ' : ' + file.description;
    }
    return title;
  }
  return '';
};

let nbFileUploading = 0;
const initUploadFile = function(ctrl, Upload, $timeout, $window, master) {
  let config = null;
  if (ctrl.element.config) {
    config = JSON.stringify(ctrl.element.config);
  }

  const uploadFiles = function(files, errFiles) {
    ctrl.files = files;
    ctrl.errFiles = errFiles;
    nbFileUploading = files.length;
    files.forEach(function(file) {
      file.upload = Upload.upload({
        url: master.getUrl('/api/file/post'),
        data: {
          file: file,
          siteId: $window.params.master.site.siteId,
          config: config ? config : undefined,
          /* headers: {
                         '__setXHR_': null 
                     }*/
        },
        disableProgress: true,
      });

      file.upload.then(
        function(response) {
          $timeout(function() {
            const files = response.data.files;
            if (files) {
              for (var i = 0; i < files.length; i++) {
                nbFileUploading--;
                ctrl.element.data.push(files[i]);
              }
            }
          });
        },
        function(response) {
          if (response.status > 0) {
            ctrl.errorMsg = response.status + ': ' + response.data;
            nbFileUploading--;
          }
        },
        function(evt) {
          file.progress = Math.min(
            100,
            parseInt((100.0 * evt.loaded) / evt.total)
          );
        }
      );
    });
  };
  return uploadFiles;
};

const isFileUploading = function() {
  return nbFileUploading > 0;
};

function initDataElement(element, destElements, moduleId) {
  //if (element.type === 'file' || element.type === 'carousel') {
  const fileElement = {
    type: element.type,
    property: element.propertyName,
    label: 'Text',
    data: [],
  };
  const fileDatas = JSON.parse(element.data);
  for (var j = 0; j < fileDatas.length; j++) {
    const serveurElement = fileDatas[j];
    const fileData = mapFileData(serveurElement, moduleId, master.site.siteId);
    fileElement.data.push(fileData);
  }
  destElements.push(fileElement);
}

const addElement = function(parentElement, guid) {
  const newElement = {
    type: 'file',
    property: guid.guid(),
    label: 'File',
    data: [],
    $parent: parentElement,
    config: {
      maxWidth: 800,
      maxHeigth: 400,
    },
  };
  return newElement;
};

export const service = {
  addElement,
  mapFileData,
  open: open,
  getClass,
  getAlt,
  isFileUploading: isFileUploading,
  initUploadFile: initUploadFile,
  initDataElement: initDataElement,
};
