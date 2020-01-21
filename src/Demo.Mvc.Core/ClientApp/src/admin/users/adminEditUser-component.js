import app from '../../app.module';
import history from '../../history';
import redux from '../../redux';
import { page } from '../../shared/services/page-factory';
import { toast as toastr } from '../../shared/services/toastr-factory';
import { adminUser } from './adminUser-factory';
import view from './adminEditUser.html';

import { dialogTags } from '../tags/dialogTags-factory';
import './adminEditUser.css';

const name = 'adminEditUser';

class Controller {
  constructor() {
    const connect = redux.getConnect();
    this.unsubscribe = connect(
      this.mapStateToThis,
      this.mapThisToProps
    )(this);
  }
  $onInit() {
    const vm = this;
    page.setTitle('Utilisateurs Edition', page.types.admin);
    const _user = adminUser.data.user;
    vm.user = _user;
    vm.roleDisabled = _user.userId === vm.userId;

    vm.rules = {
      lastName: ['required'],
      firstName: ['required'],
      birthdate: ['date'],
      mobilePhone: [
        {
          phone: {
            params: 'FR',
          },
        },
      ],
      phone: [
        {
          phone: {
            params: 'FR',
          },
        },
      ],
      mail: ['required', 'email'],
    };

    vm.roles = [
      {
        id: 0,
        label: 'Utilisateur espace privée',
        ticked: false,
      },
      {
        id: 1,
        label: 'Administrateur',
        ticked: false,
      },
    ];

    //  vm.role = _user.roles != null ? vm.roles[_user.role] : null;
    vm.roles.forEach(role => {
      if (_user.roles.indexOf(role.id) > -1) {
        role.ticked = true;
      }
    });

    const _tags = [];
    vm.inputFilters = dialogTags.initTags(
      dialogTags.model.users.tags,
      _tags,
      _user.tags
    );

    vm.isShowInvitation = true;
    vm.sendInvitation = function() {
      adminUser.sendInvitationAsync(vm.user.siteUserId).then(function() {
        toastr.success(
          'Invitation effectuée avec succès.',
          'Invitation utilisateur'
        );
        vm.isShowInvitation = false;
      });
    };

    vm.openTags = function() {
      dialogTags.openAsync('items').then(function() {
        dialogTags.initTags(dialogTags.model.users.tags, _tags, _user.tags);
      });
    };

    vm.submit = function(form) {
      if (form && form.$valid) {
        form.uMail.mw.setValidity('user_email_already_added', true);

        if (!_user.tags) {
          _user.tags = [];
        }
        _user.tags.length = 0;
        vm.inputFilters.forEach(function(tag) {
          if (tag.ticked) {
            _user.tags.push(tag.id);
          }
        }, this);

        _user.roles.length = 0;

        vm.roles.forEach(function(role) {
          if (role.ticked) {
            _user.roles.push(role.id);
          }
        }, this);

        adminUser.saveAsync(vm.user).then(function(response) {
          if (response && response.data) {
            const result = response.data;
            const errors = result.validationResult.errors;
            if (result.isSuccess) {
              toastr.success(
                'Sauvegarde effectuée avec succès.',
                'Sauvegarde utilisateur'
              );
              if (errors.no_user_found) {
                toastr.warning(
                  'Utilisateur non inscrit sur le site',
                  "Un email d'invitation a été envoyé à l'utilisateur"
                );
              }
            } else {
              if (errors.user_email_already_added) {
                toastr.warning(
                  'Utilisateur existant',
                  'Un utilisateur avec le même email à déjà été ajouté.'
                );
                form.uMail.mw.setValidity(
                  'user_email_already_added',
                  false,
                  'Un utilisateur avec le même email à déjà été ajouté.'
                );
              }
            }
            if (result.data.siteUserId && !vm.user.siteUserId) {
              history.path(
                '/administration/utilisateurs/edition/' + result.data.siteUserId
              );
            }
          }
        });
      }
    };
    return vm;
  }
  $onDestroy() {
    this.unsubscribe();
  }
  mapStateToThis(state) {
    return { userId: state.userId };
  }
  mapThisToProps() {
    return {};
  }
}

app.component(name, {
  template: view,
  controller: [Controller],
  controllerAs: 'vm',
  bindings: {},
});

export default name;
