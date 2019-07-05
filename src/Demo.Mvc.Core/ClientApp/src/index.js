import modulesFactory from './modules-factory';

const start = async () => {
    const url = window.location.toString().split('#')[0];
    const port = window.location.port ? window.location.port : "443";
    const response = await fetch(`/api/site/master?url=${encodeURI(url)}&port=${port}`);
    const result = await response.json();

    var meta = document.createElement('base');
        meta.setAttribute('href', result.header.baseUrlSite);
    document.getElementsByTagName('head')[0].appendChild(meta);
    
    window.params = result;
    await import('./app.js');
    await modulesFactory.initAsync();
    window.angular.bootstrap(window.document, ['mw']);
};

start();