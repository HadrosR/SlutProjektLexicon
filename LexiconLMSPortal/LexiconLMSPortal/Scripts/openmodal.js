$('#modalbtn').click(function (eve) {
    $('#modal-content').load('/Teacher/CreateStudent/1');
});

// Ajax Request
var ajaxFormSubmit = function (e) {
    e.preventDefault();

    var $form = $(this);

    if (!$form.validate()) {
        return;
    }

    var options = {
        url: $form.attr("action"),
        type: $form.attr("method"),
        data: $form.serialize()
    };

    $.ajax(options).done(function (data) {
        var $target = $("#StudentListPartial");
        $target.replaceWith(data);

    });
};

// Add Ajax functionality to form
$("body").on("submit", "#TheModal form", ajaxFormSubmit);