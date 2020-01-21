import toastr from 'toastr';

// Display a info toast, with no title
function info(text, title) {
  toastr.info(text, title);
}

// Display a warning toast, with no title
function warning(text, title) {
  toastr.warning(text, title);
}

// Display a success toast, with a title
function success(text, title) {
  toastr.success(text, title);
}

// Display an error toast, with a title
function error(text, title) {
  toastr.error(text, title);
}

export const toast = {
  info: info,
  warning: warning,
  success: success,
  error: error,
};
