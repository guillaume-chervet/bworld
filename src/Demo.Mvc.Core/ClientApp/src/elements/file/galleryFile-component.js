import app from '../../app.module';
import { service as fileElementService } from './elementFile-factory';
import { menuAdmin } from '../../admin/menu/menuAdmin-factory';
import { service as linkService } from '../link/elementLink-factory';
import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import { LoadableVideo } from './Video';
import { element } from 'prop-types';

import './file.css';

const name = 'galleryFile';

const FileAdmin = ({ file, element }) => {

    const open = fileElementService.open;
    const up = function (element, file) {
        menuAdmin.up(file, element.data);
    };

    const down = function (element, file) {
        menuAdmin.down(file, element.data);
    };

    const canUp = function (element, file) {
        return menuAdmin.canUp(file, element.data);
    };

    const canDown = function (element, file) {
        return menuAdmin.canDown(file, element.data);
    };

    const destroy = function (element, file) {
        menuAdmin.deleteElement(file, element.data);
    };

    return (<>
        <button type="button" className="btn btn-danger mw-image-delete" onClick={destroy(element, file)}>
            <span className="glyphicon glyphicon-trash"></span>
        </button>
        {canUp(element, file) && (<button type="button" onClick={up(element, file)} className="btn btn-primary mw-image-up">
            <span className="glyphicon glyphicon-chevron-up" aria-hidden="true"></span>
        </button>)}
        {canDown(element, file) && (<button type="button" onClick={down(element, file)} className="btn btn-primary mw-image-down">
            <span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span>
        </button>)}
        <img src={file.thumbnailUrl} className="center-block img-responsive hand" alt={file.name} onClick={open(element, file, true)} />
    </>);
}


const File = ({ file }) => {

    const getPath = function (data) {
        const url = linkService.getPath(data);
        return url;
    };
    const open = fileElementService.open;
    const getClass = fileElementService.getClass;
    const getAlt = fileElementService.getAlt;

    let content = null;

    switch (file.displayType) {
        case 'image':
            switch (file.behavior) {
                case 'noZoom':
                    content = (
                        <img alt={getAlt(file)} src={file.thumbnailUrl} className={"center-block img-responsive " + getClass(file)} />
                    );
                    break;
                case 'link':
                    content = (
                        <a href={getPath(file.link)}>
                            <img alt={getAlt(file)} src={file.thumbnailUrl} className={"center-block img-responsive " + getClass(file)} />
                        </a>
                    );
                    break;
                default:
                    content = (
                        <a onClick={() => open(element, file, false)} title={getAlt(file)} className="hand">
                            <img alt={getAlt(file)} src={file.thumbnailUrl} className={"center-block img-responsive " + getClass(file)} />
                        </a>
                    );
                    break;
            }
            break;
        case 'video':
            content = (<LoadableVideo file="file" />);
            break;
        default:
            break;
        
    }
    return (<div>{content}</div>);
}

const Files = ({ element, isAdmin }) => {
    const allFiles = element.data.map(file => {
        if (isAdmin) {
            return (<ContainerFiles element={element} ><FileAdmin key={file.thumbnailUrl} file={file} element={element} /></ContainerFiles>); }
        else { return (<ContainerFiles element={element}><File key={file.thumbnailUrl} file={file} /></ContainerFiles>); }
    });
    return (<div className="row">{allFiles}</div>);
}
        
const ContainerFiles = ({element, children}) => {

        switch (element.data.length) {
            case 1:
                return (<div className="col-lg-12 col-sm-12 col-xs-12">
                {children}
                </div>);
                break;
            case 2:
                return (<div className="col-lg-6 col-sm-6 col-xs-12">
                {children}
                </div>);
                break;
            default:
                return (<div className="col-lg-4 col-sm-6 col-md-4 col-xs-12">
                {children}
                </div>);
                break;
        }
    }

const GalleryFile = ({ element, isAdmin }) => {
    return (<div className="col-lg-12 col-sm-12 col-xs-12">
        <Files element={element} isAdmin={isAdmin} />
    </div>);
};

app.component(name, react2angular(GalleryFile, ['element', 'isAdmin']));

export default name;
