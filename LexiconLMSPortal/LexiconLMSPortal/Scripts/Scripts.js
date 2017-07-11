$(document).ready(function(){

    // Collapse Module Accordion
    $("body").on("click", ".module-accordion-heading", function (e) {
        var collapsed = $(this).nextUntil(".module-accordion-link");
        collapsed.collapse({
            'parent': "#module-accordion",
            "toggle": false
        });
        collapsed.collapse('toggle');

    });

});