import { breadcrumb } from '../../breadcrumb/breadcrumb-factory';
import { master } from '../providers/master-provider';
import { audit } from './audit-factory';

const types = {
  public: {
    id: 0,
    name: 'Publique',
  },
  private: {
    id: 1,
    name: 'Prive',
  },
  user: {
    id: 2,
    name: 'Utilisateur',
  },
  admin: {
    id: 3,
    name: 'Administration',
  },
  superAdmin: {
    id: 4,
    name: 'SuperAdministration',
  },
};

function setTitle(title, type) {
  let tracePage = '';
  if (type) {
    tracePage = `${type.name} ${title}`;
  } else {
    type = types.public;
    tracePage = title;
  }
  breadcrumb.page.title = title;
  master.updateMasterMetas({ titlePage: tracePage });
  audit.trace(tracePage, 'Page', type);
}

function setMetas(metas) {
  //let page = master.master;
  /*page.metaDescription = metas.metaDescription;
    page.metaKeyword = metas.metaKeyword;
    page.author = metas.author;
    page.title = metas.title;*/
  master.updateMasterMetas(metas);
  console.log('here in meta');
}

export const page = {
  setTitle: setTitle,
  setMetas: setMetas,
  types: types,
};
