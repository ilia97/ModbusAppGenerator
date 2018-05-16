$(function () {
    checkValidations();

    $(".mdl-textfield__input.required").blur(function () {
        if (!this.value) {
            $(this).prop('required', true);
            $(this).parent().addClass('is-invalid');
        }
    });

    $(".mdl-button[type='submit']").click(function (event) {
        var textFields = $(this).parents('form').find('.mdl-textfield__input.required');

        for (var i = 0; i < textFields.length; i++) {
            textField = textFields[i];

            if (!textField.value) {
                $(textField).parent().addClass('is-invalid');
                $(textField).prop('required', true);
            }
        }
    });
});

function checkValidations() {
    var textFieldErrors = $(".mdl-textfield__error");

    for (var i = 0; i < textFieldErrors.length; i++) {
        textFieldError = textFieldErrors[i];

        if ($(textFieldError).text()) {
            $(textFieldError).parent().addClass('is-invalid');
        }
    }
}