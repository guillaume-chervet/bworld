import app from '../../app.module';

import {react2angular} from "react2angular";
import React, {useCallback} from "react";
import {GalleryFile} from "./elementFile-component";
import "./upload.css";
import {useDropzone} from "react-dropzone";
import {master} from '../../shared/providers/master-provider';
import {elementConfig} from "./elementFile-factory";
import $http from '../../http';

const name = 'upload';


const postFileAsync = async (file, configJson, siteId, onChange, element) => {
    const data = new FormData();
    data.append('file', file)
    data.append('config', configJson)
    data.append('siteId', siteId)
    const url =master.getUrl('/api/file');

    const json = await $http.postFormDataAsync(url, data);
    
    const files = json.files;
    if (files) {
        const data = [...element.data,...files];
        onChange(data);
    }
}

function UploadDropzone({element, master, onChange}) {

  let configJson = null;
  if (element.config) {
    configJson = JSON.stringify(element.config);
  } else {
      configJson = JSON.stringify(elementConfig)
  }
  
  const onDrop = useCallback((acceptedFiles) => {
    acceptedFiles.forEach((file) => {
        postFileAsync(file, configJson, master.site.siteId, onChange, element);
    });

  }, []);
  const {getRootProps, getInputProps} = useDropzone({onDrop});

  return (
      <div {...getRootProps()}>
        <span className="fa fa-camera"/>
        Séléctionner fichiers
        <input {...getInputProps()} accept="image/*,video/mp4" multiple={true} className="btn btn-default btn-lg" />
        <p>Drag 'n' drop some files here, or click to select files</p>
      </div>
  );
}

export const Upload = ({ element, mode, onChange }) => {
  const onChangeWrapper = (data) => {
    onChange({ what: "element-edit", element: {...element, data: data}});
  };
  return (
  <div className="mw-file">
    <div className="text-center">
      <UploadDropzone master={master} element={element} onChange={onChangeWrapper}/>
    </div>
    <GalleryFile element={element} isAdmin={true} onChange={onChange} />
  </div>

);
};
