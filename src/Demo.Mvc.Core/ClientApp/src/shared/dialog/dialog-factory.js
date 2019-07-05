import $modal from '../../modal';

const openAsync = function(data) {
  let size = data.size;
  const title = data.title;

  if (!size) {
    size = 'lg';
  }

  const modalInstance = $modal.open({
    template:
      '<dialog-info close="$close()" dismiss="$dismiss()" data="$ctrl.data">' +
      data.template +
      '</dialog-info>',
    controller: function() {
      this.data = {
        title: title,
      };
      return this;
    },
    controllerAs: '$ctrl',
    size: size,
    resolve: {
      element: function() {
        return null;
      },
    },
  });

  return modalInstance;
};

export const dialog = {
  openAsync: openAsync,
};
