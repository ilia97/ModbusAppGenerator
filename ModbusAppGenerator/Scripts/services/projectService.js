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

    this.updateAction = function (action, callback) {
        $.ajax({
            type: "POST",
            url: "/Project/UpdateAction",
            data: action,
            success: function (data, textStatus, jqXHR) {
                if (callback) {
                    callback(data);
                }
            }
        });
    }

    this.deleteAction = function (actionId, projectId, callback) {
        $.ajax({
            type: "POST",
            url: "/Project/DeleteAction/",
            data: {
                projectId: projectId,
                id: actionId
            },
            success: function (data, textStatus, jqXHR) {
                if (callback) {
                    callback(data);
                }
            }
        });
    }

    this.deleteProject = function (id) {
        window.location = window.location.origin + '/Project/Delete/' + id;
    }

    this.downloadProject = function (id, type) {
        window.location = window.location.origin + '/Project/Download/' + id + '?type=' + type;
    }

    this.runOnline = function (id, cyclesCount, callback) {
        $.post(
            '/Project/Test',
            {
                id: id,
                cyclesCount: cyclesCount
            },
            function (data) {
                callback(data.Logs, data.Results);
            });
    }
}