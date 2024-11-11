async function handleErrorApi(response) {
    let errorMessage = '';

    if (response.status === 400) {
        errorMessage = await response.text();
    } else if (response.status === 404) {
        errorMessage = resourceNotFound;
    } else {
        errorMessage = unknowError;
    }

    showErrorMessage(errorMessage);
}

function showErrorMessage(message) {
    Swal.fire({
        icon: 'error',
        title: 'Error...',
        text: message
    });
}

function confirmAction({ callBackAcept, callbackCancel, title }) {
    Swal.fire({
        title: title || areYouSure,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: yes,
        focusConfirm: true
    }).then((result) => {
        if (result.isConfirmed) {
            callBackAcept();
        } else if (callbackCancel) {
            // The user has pressed the cancel button
            callbackCancel();
        }
    })
}