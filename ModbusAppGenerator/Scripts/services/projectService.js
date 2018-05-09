var ProjectService = function () {
    this.createIpProject = function (ipProject) {
        var data = ipProject;

        $.ajax({
            type: 'POST',
            url: '/Project/CreateIpProject',
            data: data,
            dataType: 'application/json',
            success: function (data) {

            },
        });
    }

    this.updateAction = function (action) {
        $.post('/Project/UpdateAction', action, function () {
            // if success
            location.reload();
        });
    }

    this.deleteAction = function (actionId, projectId) {
        window.location = window.location.origin + '/Project/DeleteAction/' + actionId + '?projectId=' + projectId;
    }

    this.deleteProject = function (id) {
        window.location = window.location.origin + '/Project/Delete/' + id;
    }

    this.downloadProject = function (id, type) {
        window.location = window.location.origin + '/Project/Download/' + id + '?type=' + type;
    }
}