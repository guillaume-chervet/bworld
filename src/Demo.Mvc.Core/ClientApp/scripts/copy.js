const fs = require('fs-extra');

// copy file  build/index.html
fs.copySync('./build/index.html', '../Views/Home/Index.cshtml');
