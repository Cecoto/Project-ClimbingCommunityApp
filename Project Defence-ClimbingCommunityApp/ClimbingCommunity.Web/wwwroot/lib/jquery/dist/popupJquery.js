$(document).ready(function () {
    // Display the lightbox popup when the "Join Now" button is clicked
    $('#join-button').click(function () {

        loadPopup();
    });
    $('#register-link').click(function () {

        loadPopup();
    });
    // Handle user type selection
    $(document).on('click', '#climber-button', function () {
        redirectToRegistration('Climber');
    });

    $(document).on('click', '#coach-button', function () {
        redirectToRegistration('Coach');
    });

    $(document).on('click', '.lightbox-container', function (e) {
        if (e.target === this) {
            closePopup();
        }
    });

    // Load the partial view into the popup placeholder
    function loadPopup() {
        // Check if the popup placeholder element exists
        var $popupPlaceholder = $('#popup-placeholder');
        if ($popupPlaceholder.length) {
            // Load the partial view into the popup placeholder
            $popupPlaceholder.load('/Home/UserTypePopup');
        }
    }

    // Redirect to the appropriate registration view based on user type
    function redirectToRegistration(userType) {
        // Add logic to redirect or load the correct registration view based on the userType
        if (userType === 'Climber') {
            window.location.href = '/User/RegisterClimber';
        } else if (userType === 'Coach') {
            window.location.href = '/User/RegisterCoach';
        }
    }

    // Close the popup and remove it from the DOM
    function closePopup() {
        $('#lightbox-container').remove();
    }
});
