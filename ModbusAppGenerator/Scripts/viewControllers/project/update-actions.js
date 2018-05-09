$(function () {
    var projectService = new ProjectService();

    var submitButton = $('#save-action-button');
    var addActionButton = $('#add-action-button');

    var projectIdInput = $('#Id');

    var writeRadioButton = $('#ActionType_Write');
    var readRadioButton = $('#ActionType_Read');

    var editButtons = $('.edit');
    var deleteButtons = $('.delete');

    var dialog = $('dialog');
    var closeDialogButton = dialog.find('.close');

    writeRadioButton.click(changeFormulaVisibility);
    readRadioButton.click(changeFormulaVisibility);

    function changeFormulaVisibility() {
        var formulaContainer = $('.formula-container');

        if (writeRadioButton.is(':checked')) {
            formulaContainer.removeClass('hidden');
        } else {
            formulaContainer.addClass('hidden');
        }
    }

    addActionButton.click(function () {
        dialog[0].showModal();
    });

    $('.update-actions').on('click', '.edit', function () {
        var actionBlock = $(this).parents('.action');

        $('#ActionId').val(actionBlock.data('id'));
        $('#SlaveAddress').val(actionBlock.data('slave-address'));
        $('#NumberOfRegisters').val(actionBlock.data('number-of-registers'));
        $('#StartAddress').val(actionBlock.data('start-address'));
        $('#Types').val(actionBlock.data('types'));
        $('#Formula').val(actionBlock.data('formula'));

        if (actionBlock.data('action-type') == 'Read') {
            $('#ActionType_Read')[0].parentNode.MaterialRadio.check();
            $('.formula-container').addClass('hidden');
        } else {
            $('#ActionType_Write')[0].parentNode.MaterialRadio.check();
            $('.formula-container').removeClass('hidden');
        }

        dialog[0].showModal();
    });

    submitButton.click(function () {
        var action = collectActionModel();

        projectService.updateAction(action);
    });

    deleteButtons.click(function () {
        var actionId = $(this).parents('.action').data('id');
        var projectId = projectIdInput.val();

        projectService.deleteAction(actionId, projectId);
    });

    function collectActionModel() {
        var actionType = 'Read';

        if ($('#ActionType_Write').prop("checked")) {
            actionType = 'Write';
        }

        var model = {
            Id: $('#ActionId').val(),
            SlaveAddress: $('#SlaveAddress').val(),
            NumberOfRegisters: $('#NumberOfRegisters').val(),
            StartAddress: $('#StartAddress').val(),
            Types: $('#Types').val(),
            ActionType: actionType,
            Formula: $('#Formula').val(),
            ProjectId: projectIdInput.val()
        };

        return model;
    }
    
    closeDialogButton.click(function () {
        dialog[0].close();
    });
});