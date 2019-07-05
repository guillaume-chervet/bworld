/*(function () {
    'use strict'; 

import $http from '../../http';
    angular.module('mw').factory('Comment', ['$q', '$route', '$location', 'Master', function ($q, $route, $location, master) {

        var data = { 
            comments: [],
            isLoading: false
        };

        var getModuleId = function () {
            return master.getModuleId($route);
        };

        var _init = function (comment) {

            data.comments.length = 0;
            if (comment && comment.comments) {
                var comments = comment.comments;
                for (var i = 0; i < comments.length; i++) {
                    data.comments.push(comments[i]);
                }
            }
        };

        var getAsync = function () {
            this.data.isLoading = true;

            var moduleId = getModuleId();
            var promise = $http.post(master.getUrl('api/comment/' + moduleId), { headers: { disableLoader: true } }).then(function (response) {
                if (response.data.isSuccess) {
                    this.data.isLoading = false;
                    _init(response.data.data.comments);
                }
                return response.data;

            });

            return promise;
        };

        var countAsync = function (data) {
            this.data.isLoading = true;
            var dataToSend = {
                data:data
            };

            var promise = $http.post(master.getUrl('api/comment/count'), dataToSend, { headers: { disableLoader: true } }).then(function (response) {
                if (response.data.isSuccess) {
                    this.data.isLoading = false;
                }
                return response.data;

            });

            return promise;
        };

        var addCommentAsync = function (data) {

            var dataToSend = {
                comment: data.comment,
                urlPath: $location.url(),
                articleTitle: master.master.titlePage,
                siteId: master.site.siteId,
                moduleId: getModuleId()
            };

            var promise = $http.post(master.getUrl('api/comment/save'), dataToSend, { headers: { loaderMessage: 'Envoi en cours...' } }).then(function (response) {
                if (response.data.isSuccess) {
                    toastr.success('Message envoyé avec succès.', 'Envoi message');

                    init(response.data.data);

                }
                return response.data;

            });

            return promise;
        };

        var deleteCommentAsync = function (commentId) {

            var dataToSend = {
                siteId: master.site.siteId,
                moduleId: getModuleId(),
                commentId: commentId
            };

            var promise = $http.post(master.getUrl('api/comment/delete'), dataToSend, { headers: { loaderMessage: 'Suppression en cours...' } }).then(function (response) {
                if (response.data.isSuccess) {
                    toastr.success('Suppression réalisée avec succès.', 'Supression commentaire');

                    init(response.data.data);

                }
                return response.data;

            });

            return promise;
        };

        return {
            countAsync: countAsync,
            getAsync: getAsync,
            addCommentAsync: addCommentAsync,
            deleteCommentAsync:deleteCommentAsync,
            data: data,
        };

    }]);
}());*/
