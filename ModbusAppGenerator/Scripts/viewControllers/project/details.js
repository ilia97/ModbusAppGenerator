$(function () {
    var projectService = new ProjectService();

    var downloadButton = $('#download-button');
    var runOnlinePopupButton = $('#run-online-button');
    var downloadProjectIdInput = $('#dialog-download-project-id');
    var downloadDialog = $('dialog.download-dialog');
    var onlineDialog = $('dialog.online-dialog');
    var runOnlineButton = $('button.run-online');
    var closeOnlineButton = onlineDialog.find('.close');
    var loader = $('.loader');
    var cyclesCountInput = $('#CyclesCount');
    var logsBlock = $('.logs');
    var resultsBlock = $('.results');

    var consoleAppButton = downloadDialog.find('.console');
    var serviceAppButton = downloadDialog.find('.service');

    downloadButton.click(function () {
        downloadDialog[0].showModal();
    });

    runOnlinePopupButton.click(function () {
        cyclesCountInput.val(1);
        logsBlock.html('');
        resultsBlock.html('');
        loader.addClass('hidden');
        runOnlineButton.prop('disabled', false);

        onlineDialog[0].showModal();
    });

    consoleAppButton.click(function () {
        projectService.downloadProject(downloadProjectIdInput.val(), 0);
        downloadDialog[0].close();
    });

    serviceAppButton.click(function () {
        projectService.downloadProject(downloadProjectIdInput.val(), 1);
        downloadDialog[0].close();
    });

    runOnlineButton.click(function () {
        loader.removeClass('hidden');
        runOnlineButton.prop('disabled', 'disabled');

        projectService.runOnline(downloadProjectIdInput.val(), cyclesCountInput.val(), function (logs, results) {
            loader.addClass('hidden');
            runOnlineButton.prop('disabled', false);

            var htmlLogs = "";

            for (var logIndex in logs) {
                htmlLogs += '<p>' + logs[logIndex] + '</p>';
            }

            var htmlResults = "";

            for (var resultIndex in results) {
                htmlResults += "Cycle " + (+resultIndex + 1) + '<br />';

                for (var register in results[resultIndex]) {
                    htmlResults += 'Register ' + register + ': data ' + results[resultIndex][register] + '<br />';
                }

                htmlResults += '<br />';
            }

            logsBlock.html(htmlLogs);
            resultsBlock.html(htmlResults);
        });
    });

    closeOnlineButton.click(function () {
        onlineDialog[0].close();
    });
});