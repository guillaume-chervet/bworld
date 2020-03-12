import app from '../../app.module';
import $window from '../../window';
import { master } from '../../shared/providers/master-provider';
import { service as fileElementService } from './elementFile-factory';
import view from './upload.html';

const name = 'upload';

class Controller {
  constructor(Upload, $timeout) {
    this.Upload = Upload;
    this.$timeout = $timeout;
  }
  $onInit() {
    const ctrl = this;

    ctrl.uploadFiles = fileElementService.initUploadFile(
      ctrl,
      this.Upload,
      this.$timeout,
      $window,
      master
    );
    ctrl.isFileUploading = fileElementService.isFileUploading;

    return ctrl;
  }
}

app.component(name, {
  template: view,
  controller: ['Upload', '$timeout', Controller],
  bindings: {
    element: '=',
    onChange: '<',
  },
});

export default name;
