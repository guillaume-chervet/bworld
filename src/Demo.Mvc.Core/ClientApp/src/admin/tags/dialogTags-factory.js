import _ from 'lodash';

import { master } from '../../shared/providers/master-provider';
import $http from '../../http';
import $modal from '../../modal';
import $q from '../../q';

const model = {
  users: { tags: [], isInit: false },
  items: { tags: [], isInit: false },
};

const openAsync = function(type) {
  if (!type) {
    type = 'users';
  }
  const modalInstance = $modal.open({
    animation: true,
    template:
      '<dialog-tags close="$close()" dismiss="$dismiss()" data="$ctrl.data"></dialog-tags>',
    controller: function() {
      this.data = {
        type,
        tags: model[type].tags,
      };
      return this;
    },
    size: 'md',
    controllerAs: '$ctrl',
    resolve: {
      element: function() {
        return null;
      },
    },
  });
  return modalInstance.result;
};

const saveAsync = function(tags, type) {
  if (!type) {
    type = 'users';
  }

  const siteId = master.site.siteId;
  const postData = {
    siteId,
    type,
    tags,
  };

  return $http
    .post(master.getUrl('api/tags'), postData)
    .then(function(response) {
      if (response.data.isSuccess) {
        const tagsDestination = model[type].tags;
        tags.forEach(function(tag) {
          const oldTag = _.find(tagsDestination, function(o) {
            return o.id === tag.id;
          });
          if (oldTag) {
            Object.assign(oldTag, tag);
          } else {
            tagsDestination.push(_.cloneDeep(tag));
          }
        });
        return tags;
      }
      return null;
    });
};

var initAsync = function(type) {
  if (!type) {
    type = 'users';
  }

  var tags = model[type].tags;
  if (!model[type].isInit) {
    model[type].isInit = true;

    tags.length = 0;
    const siteId = master.site.siteId;
    return $http
      .get(master.getUrl('api/tags/' + siteId + '/' + type))
      .then(function(response) {
        if (response && response.data.data) {
          const data = response.data.data;
          if (data.tags.length > 0) {
            Object.assign(tags, _.cloneDeep(data.tags));
          } else {
            switch (type) {
              case 'users':
                {
                  const initTag = [
                    {
                      id: '1',
                      name: 'Client',
                      isDeleted: false,
                    },
                    {
                      id: '2',
                      name: 'Prospet',
                      isDeleted: false,
                    },
                    {
                      id: '3',
                      name: 'Amis',
                      isDeleted: false,
                    },
                    {
                      id: '4',
                      name: 'Famille',
                      isDeleted: false,
                    },
                    {
                      id: '5',
                      name: 'Connaissance',
                      isDeleted: false,
                    },
                  ];
                  Object.assign(tags, _.cloneDeep(initTag));
                }
                break;

              default:
                {
                  const initTag = [
                    {
                      id: '1',
                      name: 'News',
                      isDeleted: false,
                    },
                    {
                      id: '2',
                      name: 'Article',
                      isDeleted: false,
                    },
                  ];
                  Object.assign(tags, _.cloneDeep(initTag));
                }
                break;
            }
          }
        }
        return tags;
      });
  } else {
    const deferred = $q.defer();
    const promise = deferred.promise;
    deferred.resolve(tags);
    return promise;
  }
};

const getTagInfo = function(tagId, type) {
  if (!type) {
    type = 'users';
  }
  const sourceTags = model[type].tags;
  if (sourceTags) {
    for (var o = 0; o < sourceTags.length; o++) {
      const _sourceTag = sourceTags[o];
      if (!_sourceTag.isDeleted && _sourceTag.id === tagId) {
        return {
          id: _sourceTag.id,
          name: _sourceTag.name,
        };
      }
    }
  }

  return {
    id: null,
    name: 'Not found',
  };
};

const initTags = function(sourceTags, destinationTags, selectedTags) {
  destinationTags.length = 0;
  sourceTags.forEach(function(_sourceTag) {
    if (!_sourceTag.isDeleted) {
      destinationTags.push({
        id: _sourceTag.id,
        name: _sourceTag.name,
        ticked: false,
      });
    }
  }, this);
  if (selectedTags) {
    destinationTags.forEach(function(tag) {
      if (selectedTags.indexOf(tag.id) > -1) {
        tag.ticked = true;
      }
    }, this);
  }
  return destinationTags;
};

export const dialogTags = {
  openAsync: openAsync,
  saveAsync: saveAsync,
  initAsync: initAsync,
  model: model,
  getTagInfo: getTagInfo,
  initTags: initTags,
};
