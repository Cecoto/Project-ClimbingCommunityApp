// myTrainings.js

document.addEventListener('DOMContentLoaded', function () {
    const commentsButtons = document.querySelectorAll('.comments-button');
    commentsButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            const trainingId = button.dataset.trainingId;
            const commentsContainer = document.getElementById('comments-' + trainingId);
            commentsContainer.classList.toggle('show');
        });
    });
});

