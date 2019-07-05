/*(function () {
    'use strict';

    angular.module('mw').controller('CommentController', ['$scope', 'Comment', 'User', function ($scope, comment, user) {

        $scope.comments = comment.data.comments;
        $scope.user = user.user;
        $scope.model = {
            message: ''
        };

        $scope.rules = {
            message: ['required']
        };

        $scope.submit = function () {

            if ($scope.form.$valid) {

                var data = {
                    comment: $scope.model.message
                };

                comment.addCommentAsync(data).then(function () {
                    $scope.messageSended = true;
                });
            }
        };

        $scope.delete = function (commentId) {

            if (commentId) {

                var isConfirm = confirm('Etes-vous sûr de vouloir supprimer ce commentaire?');
                if (!isConfirm) {
                    return null;
                }

                comment.deleteCommentAsync(commentId);
            }
        };
        
        $scope.messageSended = false;

        $scope.initMessage = function() {
            $scope.model.message = '';
            $scope.messageSended = false;
            $scope.form.$setPristine();
        };

    }]);

}());
*/
