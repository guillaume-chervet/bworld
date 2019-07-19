import app from '../../app.module';
import redux from '../../redux';
import { audit } from '../../shared/services/audit-factory';
import { service as linkService } from '../link/elementLink-factory';
import view from './modalImage.html';
import viewAdmin from './modalImage_admin.html';

const name = 'modalImage';

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    const ctrl = this;

    const currentElement = ctrl.data.element;
    const parent = ctrl.data.parent;

    ctrl.menuItems = linkService.init();

    const images = [];

    function pushImage(value) {
      if (value.displayType === 'image') {
        images.push(value);
      }
    }

    for (let o = 0; o < parent.childs.length; o++) {
      const child = parent.childs[o];
      if (child.type === 'file' || child.type === 'image') {
        child.data.forEach(pushImage);
      }
    }

    const index = images.indexOf(currentElement);
    /*	var sortedImages = images.slice(index, images.length);
            for (var a = 0; a < index; a++) {
                sortedImages.push(images[a]);
            }*/
    // Current active slide
    ctrl.active = index;

    const slides = [];
    ctrl.slides = slides;
    for (let i = 0; i < images.length; i++) {
      const elem = images[i];
      slides.push({
        url: elem.url,
        file: elem,
        id: i,
      });
    }

    ctrl.getSlide = function(active) {
      for (let j = 0; j < slides.length; j++) {
        const slide = slides[j];
        if (slide.id === active) {
          return slide;
        }
      }
      return null;
    };

    ctrl.getTitle = function(active) {
      const slide = ctrl.getSlide(active);
      let title = '';
      if (slide) {
        if (!slide.file.title) {
          title = slide.file.name;
        } else {
          title = slide.file.title;
        }
      }
      // TODO mettre ailleur
      audit.trace(ctrl.titlePage, 'Image ' + title);
      return title;
    };

    ctrl.ok = function() {
      ctrl.close();
    };

    ctrl.cancel = function() {
      ctrl.dismiss('cancel');
    };
    return ctrl;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return {
      titlePage: state.master.masterData.titlePage,
    };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view ,
  controller: [Controller],
  bindings: {
    resolve: '<',
    close: '&',
    dismiss: '&',
    data: '=',
  },
});

app.component(name + 'Admin', {
  template: viewAdmin,
  controller: [Controller],
  bindings: {
    resolve: '<',
    close: '&',
    dismiss: '&',
    data: '=',
  },
});

export default name;
