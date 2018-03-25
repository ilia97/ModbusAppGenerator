$(function () {
    var projectService = new ProjectService();

    var deletetButton = $('.project-delete');
    var dialog = $('dialog');

    var closeDialogButton = dialog.find('.close');
    var confirmDialogButton = dialog.find('.confirm');
    var dialogTextBlock = dialog.find('#dialog-body');
    var deleteProjectIdInput = dialog.find('#dialog-delete-project-id');

    deletetButton.click(function () {
        deleteProjectIdInput.val($(this).data('id'));
        dialogTextBlock.html('Are you sure you want to delete project "' + $(this).data('title') + '"?');

        dialog[0].showModal();
    });

    closeDialogButton.click(function () {
        dialog[0].close();
    });

    confirmDialogButton.click(function () {
        projectService.deleteProject(deleteProjectIdInput.val());
    });
});