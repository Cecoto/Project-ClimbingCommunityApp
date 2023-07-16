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
            var url = baseUrl + "/" + tripId;
            $.ajax({
                url: url,
                type: 'POST',
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                success: function () {
                    
                    swal.fire({
                        title: 'Deleted!',
                        text: 'Your file has been deleted.',
                        icon: 'success',
                        showConfirmButton: false 
                    });
                   
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);

                },
                error: function () {      
                    swal.fire('Error', 'Failed to delete the trip.', 'error');
                }
            });
        }
    });
}
