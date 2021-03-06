﻿$("body").on("click", '#modalbtn', function (eve) {
    eve.preventDefault();
    $('#modal-content').load($(this).data("url"));
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