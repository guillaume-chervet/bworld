import { service as fileElementService } from './elementFile-factory';
import { menuAdmin } from '../../admin/menu/menuAdmin-factory';
import { service as linkService } from '../link/elementLink-factory';
import React from 'react';
import { LoadableVideo } from './Video';

import './file.css';

const FileAdmin = ({ file, element, onChange }) => {

  const onChangeWrapper = (data) => {
    onChange({ what: "element-edit", element: {...element, data}});
  };

  const doAction = (action) => {
    const newData =  action(file, element.data);
    onChangeWrapper(newData);
  }
  
  const open = () => fileElementService.open(element, file, true);
  const up = () => doAction(menuAdmin.up);
  const down = () => doAction(menuAdmin.down);
  const canUp = () => menuAdmin.canUp(file, element.data);
  const canDown = () => menuAdmin.canDown(file, element.data);
  const destroy = () => doAction(menuAdmin.deleteElement);
  return (
    <>
      <button
        type="button"
        className="btn btn-danger mw-image-delete"
        onClick={destroy}>
        <span className="glyphicon glyphicon-trash"/>
      </button>
      {canUp() && (
        <button
          type="button"
          onClick={up}
          className="btn btn-primary mw-image-up">
          <span
    className="glyphicon glyphicon-chevron-up"
    aria-hidden="true"/>
        </button>
      )}
      {canDown() && (
        <button
          type="button"
          onClick={down}
          className="btn btn-primary mw-image-down">
          <span
    className="glyphicon glyphicon-chevron-down"
    aria-hidden="true"/>
        </button>
      )}
      <img
        src={file.thumbnailUrl}
        className="center-block img-responsive hand"
        alt={file.name}
        onClick={open}
      />
    </>
  );
};

const File = ({ file, element }) => {
  const getPath = linkService.getPath;
  const open = () => fileElementService.open(element, file, false);
  const getClass = fileElementService.getClass;
  const getAlt = fileElementService.getAlt;

  let content = null;

  switch (file.displayType) {
    case 'image':
      switch (file.behavior) {
        case 'noZoom':
          content = (
            <img
              alt={getAlt(file)}
              src={file.thumbnailUrl}
              className={'center-block img-responsive ' + getClass(file)}
            />
          );
          break;
        case 'link':
          content = (
            <a href={getPath(file.link)}>
              <img
                alt={getAlt(file)}
                src={file.thumbnailUrl}
                className={'center-block img-responsive ' + getClass(file)}
              />
            </a>
          );
          break;
        default:
          content = (
            <a onClick={open} title={getAlt(file)} className="hand">
              <img
                alt={getAlt(file)}
                src={file.thumbnailUrl}
                className={'center-block img-responsive ' + getClass(file)}
              />
            </a>
          );
          break;
      }
      break;
    case 'video':
      content = <LoadableVideo file="file" />;
      break;
    default:
      break;
  }
  return <div>{content}</div>;
};

const Files = ({ element, isAdmin, onChange=null }) => {
  const allFiles = element.data.map(file => {
    if (isAdmin) {
      return (
        <ContainerFiles element={element}>
          <FileAdmin key={file.thumbnailUrl} file={file} element={element} onChange={onChange} />
        </ContainerFiles>
      );
    } else {
      return (
        <ContainerFiles element={element}>
          <File key={file.thumbnailUrl} file={file} element={element} />
        </ContainerFiles>
      );
    }
  });
  return <div className="row">{allFiles}</div>;
};

const ContainerFiles = ({ element, children }) => {
  switch (element.data.length) {
    case 1:
      return <div className="col-lg-12 col-sm-12 col-xs-12">{children}</div>;
    case 2:
      return <div className="col-lg-6 col-sm-6 col-xs-12">{children}</div>;
    default:
      return (
        <div className="col-lg-4 col-sm-6 col-md-4 col-xs-12">{children}</div>
      );
  }
};

export const GalleryFile = ({ element, isAdmin, onChange=null }) => {
  return (
    <div className="col-lg-12 col-sm-12 col-xs-12">
      <Files element={element} isAdmin={isAdmin} onChange={onChange} />
    </div>
  );
};

