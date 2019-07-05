module.exports = function override(config, env) {
  console.log(config);
  config.module.rules.push({
    test: /\.html$/,
    loader: 'raw-loader',
  });

  return config;
};
