import app from '../../app.module';

import {react2angular} from "react2angular";
import React, {useCallback} from "react";
import {GalleryFile} from "./elementFile-component";
import "./upload.css";
import {useDropzone} from "react-dropzone";

const name = 'upload';


function MyDropzone() {
  const onDrop = useCallback((acceptedFiles) => {
    acceptedFiles.forEach((file) => {
      const reader = new FileReader()

      reader.onabort = () => console.log('file reading was aborted')
      reader.onerror = () => console.log('file reading has failed')
      reader.onload = () => {
        // Do whatever you want with the file contents
        const binaryStr = reader.result
        console.log(binaryStr)
      }
      reader.readAsArrayBuffer(file)
    })

  }, [])
  const {getRootProps, getInputProps} = useDropzone({onDrop})

  return (
      <div {...getRootProps()}>
        <span className="fa fa-camera"/>
        Séléctionner fichiers
        <input {...getInputProps()} accept="image/*,video/mp4" multiple={true} className="btn btn-default btn-lg" />
        <p>Drag 'n' drop some files here, or click to select files</p>
      </div>
  )
}

export const Upload = ({ element, mode, onChange }) => {
  const onChangeWrapper = (e) => {
    onChange({ what: "element-edit", element: {...element, data: {url: e.target.value}}});
  };
  return (
  <div className="mw-file">
    <div className="text-center">
      <MyDropzone/>
    </div>
    <GalleryFile element={element} isAdmin={true} />
  </div>

);
};

app.component(name, react2angular(Upload, ['element', 'mode', 'onChange']));

export default name;
