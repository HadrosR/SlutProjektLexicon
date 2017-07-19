function isEmpty(el) {
    return $.trim(el.html())
}
if (isEmpty($('#alertBox'))) {
    $('#alertBox').slideDown().delay(2000).slideUp();
}