let _modules;

const initAsync = async () => {
  const News = await import('./news');
  const Free = await import('./free');
  const Social = await import('./social');
  const Contact = await import('./contact');
  const AddSite = await import('./addSite');
  const modules = {
    News,
    Free,
    Social,
    Contact,
    AddSite,
  };
  _modules = modules;
};

const getModule = name => _modules[name].default;

export default {
  getModule,
  initAsync,
};
