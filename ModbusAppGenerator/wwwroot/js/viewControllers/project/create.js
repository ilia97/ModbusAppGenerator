$.ready(function () {
    var projectService = new ProjectService();

    var submitButton = $('#submit-button');

    var nameInput = $('#Name');
    var descriptionInput = $('#Description');
    var connectionTypeSelect = $('#connection-type');

    var ipConfigurationSettingsArea = $('#ip-configuration-settings');
    var comConfigurationSettingsArea = $('#com-configuration-settings');

    connectionTypeSelect.change(function (sender, event) {
        switch ($(this).val()) {
            case 'IP':
                ipConfigurationSettingsArea.hide();
                ipConfigurationSettingsArea.hide();
        }
    });

    submitButton.click(function () {
        switch (connectionTypeSelect.val()) {
            case 'IP':
                var ipProject = {

                };

                projectService.createIpProject(ipProject);
        }
    });
});