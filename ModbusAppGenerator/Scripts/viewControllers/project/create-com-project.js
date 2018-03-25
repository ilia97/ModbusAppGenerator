$.ready(function () {
    var projectService = new ProjectService();

    var submitButton = $('#create-project-btn');

    var idInput = $('#Id');
    var portNameInput = $('#PortName');
    var baudRateInput = $('#BaudRate');
    var dataBitsInput = $('#DataBits');
    var parityInput = $('#Parity');
    var stopBitsInput = $('#StopBits');

    submitButton.click(function () {
        var project = {
            Id: idInput.val(),
            PortName: portNameInput.val(),
            BaudRate: baudRateInput.val(),
            DataBits: dataBitsInput.val(),
            Parity: parityInput.val(),
            StopBits: stopBitsInput.val()
        };

        projectService.createComProject(project);
    });
});