$(document).ready(function () {

    var openModal;

    $('body').on('show.bs.collapse', '.panel-collapse', function (e){

        var a = $(e.currentTarget).parent().find(".glyphicon-menu-down").removeClass("glyphicon-menu-down").addClass("glyphicon-menu-up");
    })

    $('body').on('hide.bs.collapse', '.panel-collapse', function (e) {

        var a = $(e.currentTarget).parent().find(".glyphicon-menu-up").removeClass("glyphicon-menu-up").addClass("glyphicon-menu-down");
        console.log(a);
    })

    // Collapse Module Accordion ALTERNATIVE
    //$("body").on("click", ".module-accordion-heading", function (e) {
    //    var collapsed = $(this).nextUntil(".module-accordion-link");
    //    collapsed.collapse({
    //        'parent': "#module-accordion",
    //        "toggle": false
    //    }).height('auto');
    //    collapsed.collapse('toggle');

    //});

    // Ajax Request
    var ajaxFormSubmit = function (e) {
        e.preventDefault();

        var $form = $(this);

        if (!$form.validate()) {
            return;
        }

        var options;

        if ($form.attr("enctype") == "multipart/form-data")
        {
            options = {
                url: this.action,
                type: this.method,
                data: new FormData(this),
                cache: false,
                contentType: false,
                processData: false,
            }
        }
        else
        {
            var options = {
                url: $form.attr("action"),
                type: $form.attr("method"),
                data: $form.serialize(),

            };
        }

        $('body').on('click', '#sendButton', function () { 
            $(document).ajaxSuccess(function () {
                $('#alertBox').slideDown().delay(2000).slideUp();
            })
        });

        $.ajax(options).done(function (data) {
            openModal.modal('hide');
            var $target = $($form.attr("data-lms-target"));
            $target.replaceWith(data);            
        });
    };

    // Add Ajax functionality to form
    $("body").on("submit", "form[data-lms-ajax='true']", ajaxFormSubmit);

    // Ajax Links
    $("body").on("click", ".ajax-link", function (e) {
        e.preventDefault();
        openModal = $($(this).data("target"));
        var link = $(this);

        var options = {
            url: link.attr("href"),
            type: "GET",
        }

        $.ajax(options).done(function (data) {
            var $target = $($(link).data("target"));
            openModal.replaceWith(data);

            openModal = $($(link).data("target"));

            openModal.modal('show');

            $.validator.unobtrusive.parse(openModal.find("form"));
        });
    });

    // Ajax Edit Links
    $("body").on("click", ".ajax-link-edit", function (e) {
        e.preventDefault();
        openModal = $($(this).data("target"));
        var link = $(this);

        var options = {
            url: link.attr("href"),
            type: "GET"
        }

        $.ajax(options).done(function (data) {
            var $target = $($(link).data("target"));
            openModal.replaceWith(data);

            openModal = $($(link).data("target"));

            openModal.modal('show');

            $.validator.unobtrusive.parse(openModal.find("form"));
        });
    });

    // Ajax Remove Links
    $("body").on("click", ".ajax-link-remove", function (e) {
        e.preventDefault();
        openModal = $($(this).data("target"));
        var link = $(this);

        var options = {
            url: link.attr("href"),
            type: "GET"
        }

        $.ajax(options).done(function (data) {
            var $target = $($(link).data("target"));
            openModal.replaceWith(data);

            openModal = $($(link).data("target"));

            openModal.modal('show');
        });
    });
});