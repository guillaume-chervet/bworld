const cssClass = function(element, isNoClass) {
  if (isNoClass) {
    return '';
  }

  const parent = element.$parent;
  const childs = parent.childs;

  const l = childs.length;
  let cssClass = 'col-sm-offset-3 col-sm-6 col-xs-offset-0 col-xs-12';
  if (l === 2) {
    cssClass = 'col-xs-12 col-sm-6';
  } else if (l === 3) {
    cssClass = 'col-xs-12 col-sm-6 col-md-4 col-lg-4';
  } else if (l > 3) {
    cssClass = 'col-xs-12 col-sm-6 col-md-4 col-lg-3';
  }
  return cssClass;
};

export const service = {
  cssClass,
};
