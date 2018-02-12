$(function () {
    var projectService = new ProjectService();

    var submitButton = $('#save-actions-button');
    var addActionButton = $('#add-action-button');

    var projectIdInput = $('#Id');

    var actionsListDiv = $('.actions');
    var updateActionsPage = $('.update-actions');

    var newActionHtml = '<div class="action new-action">' + $('.new-action').html() + '</div>';

    updateActionsPage.on('focus', '[name=SlaveAddress]', function () {
        $(this).addClass('is-focused');
    });

    submitButton.click(function () {
        var project = getActionsModel();

        projectService.updateActions(project);
    });

    addActionButton.click(function () {
        var newAction = $(newActionHtml);

        var actionsCount = $('.action').length;
        var slaveAddressInputId = 'SlaveAddress_' + actionsCount;
        var numberOfRegistersInputId = 'NumberOfRegisters_' + actionsCount;
        var startAddressInputId = 'StartAddress_' + actionsCount;
        var typesInputId = 'Types_' + actionsCount;

        newAction.find('.action-slave-address-input').attr('id', slaveAddressInputId);
        newAction.find('.action-number-of-registers-input').attr('id', numberOfRegistersInputId);
        newAction.find('.action-start-address-input').attr('id', startAddressInputId);
        newAction.find('.action-types-input').attr('id', typesInputId);

        newAction.find('.action-slave-address-label').attr('for', slaveAddressInputId);
        newAction.find('.action-number-of-registers-label').attr('for', numberOfRegistersInputId);
        newAction.find('.action-start-address-label').attr('for', startAddressInputId);
        newAction.find('.action-types-label').attr('for', typesInputId);

        actionsListDiv.append(newAction[0].outerHTML);
    });

    function getActionsModel() {
        var model = {
            Id: projectIdInput.val(),
            Actions: []
        };

        $('.action').each(function (index, element) {
            model.Actions.push({
                Id: $(element).find('.action-id-input').val(),
                SlaveAddress: $(element).find('.action-slave-address-input').val(),
                NumberOfRegisters: $(element).find('.action-number-of-registers-input').val(),
                StartAddress: $(element).find('.action-start-address-input').val(),
                Types: $(element).find('.action-types-input').val(),
            });
        });

        return model;
    }
});