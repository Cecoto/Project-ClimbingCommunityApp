function deleteTrip(tripId) {
    swal.fire({
        title: 'Are you sure?',
        text: 'This action cannot be undone.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Delete',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            var baseUrl = document.getElementById('baseUrl').value;
            var url = baseUrl +"/"+ tripId;
            $.ajax({
                url: url,
                type: 'POST',
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                success: function () {
                    // Refresh the page or perform any additional actions
                    window.location.reload();
                },
                error: function () {
                    // Handle the error case            
                    swal.fire('Error', 'Failed to delete the trip.', 'error');
                }
            });
        }
    });
}
