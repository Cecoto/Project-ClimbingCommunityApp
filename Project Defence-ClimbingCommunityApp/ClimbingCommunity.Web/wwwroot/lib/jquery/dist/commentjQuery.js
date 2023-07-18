$(document).ready(function () {
    $('.comments-button').click(function () {
        $(this).closest('.card').find('.comments-container').slideToggle();
    });
});
