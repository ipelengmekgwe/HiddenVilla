window.ShowToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Successful", {  timeOut: 5000 })
    }

    if (type === "error") {
        toastr.error(message, "Failed", { timeOut: 5000 })
    }

    if (type === "warning") {
        toastr.warning(message, "Warning", { timeOut: 5000 })
    }

    if (type === "info") {
        toastr.info(message, "Info", { timeOut: 5000 })
    }
}

window.ShowSwal = (type, message, title = null) => {
    if (type === "success") {
        Swal.fire(title ?? 'Successful', message, 'success')
    }

    if (type === "error") {
        Swal.fire(title ?? 'Error', message, 'error')
    }

    if (type === "warning") {
        Swal.fire(title ?? 'Warning', message, 'warning')
    }

    if (type === "info") {
        Swal.fire(title ?? 'Info', message, 'info')
    }

    if (type === "question") {
        Swal.fire(title ?? 'Question', message, 'question')
    }
}

function ShowDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('show');
}

function HideDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('hide');
}