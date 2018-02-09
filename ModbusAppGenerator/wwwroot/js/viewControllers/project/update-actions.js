$(function () {
    var projectService = new ProjectService();

    var submitButton = $('#save-actions-button');
    var addActionButton = $('#add-action-button');

    var projectIdInput = $('#Id');

    var actionsListDiv = $('.actions');

    var newActionHtml = '<div class="action new-action">' + $('.new-action').html() + '</div>';

    submitButton.click(function () {
        var project = getActionsModel();

        projectService.updateActions(project);
    });

    addActionButton.click(function () {
        actionsListDiv.append(newActionHtml);
    });

    function getActionsModel() {
        var model = {
            Id: projectIdInput.val(),
            Actions: []
        };

        $('.action').each(function (index, element) {
            model.Actions.push({
                Id: $(element).find('.action-id').val(),
                SlaveAddress: $(element).find('.action-slave-address').val(),
                NumberOfRegisters: $(element).find('.action-number-of-registers').val(),
                StartAddress: $(element).find('.action-start-address').val(),
                Types: $(element).find('.action-types').val(),
            });
        });

        return model;
    }
});