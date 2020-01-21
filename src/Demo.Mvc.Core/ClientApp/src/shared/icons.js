const icons = {
  Contact: 'glyphicon glyphicon-envelope',
  Home: 'glyphicon glyphicon-home',
  Private: 'fa fa-user-secret',
  AddSite: 'glyphicon glyphicon-plus',
  News: 'fa fa-newspaper-o',
  NewsItem: 'fa fa-file-o',
  Free: 'fa fa-file-o',
};

export function getIconBase(module) {
  if (!module) {
    module = '';
  }
  module = module.toLowerCase();
  switch (module) {
    case 'contact':
      return icons.Contact;
    case 'home':
      return icons.Home;
    case 'private':
      return icons.Private;
    case 'news':
      return icons.News;
    case 'newsitem':
      return icons.NewsItem;
    case 'free':
      return icons.Free;
    case 'stats':
      return 'glyphicon glyphicon-stats';
    case 'seo':
      return 'glyphicon glyphicon-globe';
    case 'superadministration':
      return 'glyphicon glyphicon-cog';
    case 'administration':
      return 'glyphicon glyphicon-cog';
    case 'addsite':
      return 'glyphicon glyphicon-plus';
    case 'menu':
      return 'fa fa-bars';
    case 'site':
      return 'fa fa-camera';
    case 'addsite':
      return icons.AddSite;
    case 'social':
      return 'fa fa-share-alt';
    case 'user':
      return 'glyphicon glyphicon-user';
    case 'notifications':
      return 'fa fa-paper-plane';
    case 'notificationsitem':
      return 'fa fa-paper-plane-o';
    default:
      return '';
  }
}
export const getIcon = function(item) {
  if (item.module === icons.Home) {
    return icons.Home;
  }
  if (item.icon) {
    return item.icon;
  }

  return getIconBase(item.module);
};

export default icons;
