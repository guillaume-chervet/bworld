import './config';
import './news/news-component';
import './tags-component';
import './news/newsAdmin-component';
import './newsItem/newsItem-component';
import './newsItem/newsItemAdmin-component';
import './newsMenuItemAdmin-component';
import './newsMenuItem-component';
import './newsMenuItemRight-component';
import { news } from './news/news-factory';
import iconUrl from './icon.png';

import './news.css';

export default {
  canBeChild: true,
  canHaveChild: false,
  canBeParent: true,
  service: news,
  iconUrl,
};
