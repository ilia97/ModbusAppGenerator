$(function () {
    var projectService = new ProjectService();

    var downloadButton = $('#download-button');
    var downloadProjectIdInput = $('#dialog-download-project-id');
    var dialog = $('dialog');

    var consoleAppButton = dialog.find('.console');
    var serviceAppButton = dialog.find('.service');

    downloadButton.click(function () {
        dialog[0].showModal();
    });

    consoleAppButton.click(function () {
        projectService.downloadProject(downloadProjectIdInput.val(), 0);
        dialog[0].close();
    });

    serviceAppButton.click(function () {
        projectService.downloadProject(downloadProjectIdInput.val(), 1);
        dialog[0].close();
    });
});