import app from '../../app.module';
const name = 'mwHelpInfo';

function Controller() {}

app.component(name, {
  template:
    '<div class="col-sm-3 hidden-xs"><a class="mw-help" role="button" uib-popover="{{$ctrl.text}}" popover-placement="left" popover-title="Information">i</a></div>',
  controller: Controller,
  bindings: {
    text: '<content',
  },
});

export default name;
