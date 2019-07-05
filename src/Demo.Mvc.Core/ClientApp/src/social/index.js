import './config';
import './social.css';
import './socialAdmin-component';
import './socialMenuItemAdmin-component';
import './socialMenuItem-component';
import './socialMenuItemRight-component';
import { social } from './social-factory';
import iconUrl from './icon.png';

export default {
  canBeChild: false,
  service: social,
  iconUrl,
};
