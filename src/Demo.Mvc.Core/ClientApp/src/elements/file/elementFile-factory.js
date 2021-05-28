import modal from '../../modal';
import {master} from '../../shared/providers/master-provider';
import {urlHelper} from '../../shared/services/urlHelper-factory';
import { guid } from '../../shared/services/guid-factory';

const getFileExtention = filename => filename.substr(filename.lastIndexOf('.') + 1);

const mapFileData = (serveurElement, moduleId, siteId) => {
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
  let fileName;
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
        `/api/file/get/${siteId}/${serveurElement.Id}/ImageUploaded/${fileName}`;
      fileData.thumbnailUrl =
        `/api/file/get/${siteId}/${serveurElement.Id}/ImageThumb/${fileName}`;
      fileData.deleteUrl =
        `/api/file/delete/${siteId}/${serveurElement.Id}/${fileName}`;
    } else {
      fileData.url =
        `/api/file/get/${siteId}/${moduleId}/${serveurElement.PropertyName}/ImageUploaded/${fileName}`;
      fileData.thumbnailUrl =
        `/api/file/get/${siteId}/${moduleId}/${serveurElement.PropertyName}/ImageThumb/${fileName}`;
      fileData.deleteUrl =
        `/api/file/delete/${siteId}/${moduleId}/${serveurElement.PropertyName}/${fileName}`;
    }
  }

  fileData.deleteType = 'DELETE';
  fileData.isTemporary = false;

  return fileData;
};

const open = (element, file, isAdmin) => {
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

const getClass = file => {
  if (file && file.thumbDisplayMode) {
    return 'img-' + file.thumbDisplayMode;
  }
  return '';
};

const getAlt = file => {
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

function initDataElement(element, destElements, moduleId) {
  const fileElement = {
    type: element.type,
    property: element.propertyName ? element.propertyName : guid.guid(),
    label: 'File',
    data: [],
  };
  const fileDatas = JSON.parse(element.data);
  for (let j = 0; j < fileDatas.length; j++) {
    const serveurElement = fileDatas[j];
    const fileData = mapFileData(serveurElement, moduleId, master.site.siteId);
    fileElement.data.push(fileData);
  }
  destElements.push(fileElement);
}

export const elementConfig = {
  maxWidth: 800,
  maxHeigth: 400,
};
const addElement = (parentElement, guid) => {
  return {
    type: 'file',
    property: guid.guid(),
    label: 'File',
    data: [],
    $parent: parentElement,
    config: elementConfig,
  };
};

export const service = {
  addElement,
  mapFileData,
  open,
  getClass,
  getAlt,
  initDataElement,
};
