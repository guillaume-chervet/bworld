import app from '../../app.module';
const name = 'empty';

function Controller() {}

app.component(name, {
  template:
    '<div class="mw-empty" ng-if="$ctrl.items.length<=0"><p>{{$ctrl.text}}</p></div>',
  controller: Controller,
  bindings: {
    items: '=emptyItems',
    text: '=emptyText',
  },
});

export default name;
