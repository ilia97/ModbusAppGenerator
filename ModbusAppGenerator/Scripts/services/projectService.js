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

    this.updateActions = function (project) {
        $.post('/Project/UpdateActions', project, function () {
            // if success
            window.location = window.location.origin + '/Project/Details/' + project.Id;
        });
    }

    this.deleteProject = function (id) {
        window.location = window.location.origin + '/Project/Delete/' + id;
    }

    this.downloadProject = function (id, type) {
        window.location = window.location.origin + '/Project/Download/' + id + '?type=' + type;
    }
}