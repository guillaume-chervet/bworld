import './config';
import './freeMenuItemAdmin-component';
import './freeMenuItem-component';
import './freeMenuItemRight-component';
import './free-component';
import './freeAdmin-component';
import './add/dialogAddElement-component';
import './bottom/mwBottom-component';
import './user/mwUser-component';
import { free } from './free-factory';
import './free-reducer';
import iconUrl from './icon.png';

export default {
  canBeChild: true,
  canHaveChild: true,
  canBeParent: true,
  service: free,
  iconUrl,
};
