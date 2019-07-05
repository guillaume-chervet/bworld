import './config';

import './addSiteBreabcrumb-component';
import './addSite-component';
import './addSiteAdmin-component';
import './addSiteConfiguration-component';
import './addSiteAuthentification-component';
import './addSiteConfirm-component';
import './addSiteSubmit-component';
import './addSiteMenuItemAdmin-component';
import './addSiteMenuItem-component';
import './addSiteMenuItemRight-component';
import { addSite } from './addSite-factory';

export default {
  canBeChild: true,
  canBeParent: false,
  service: addSite,
};
