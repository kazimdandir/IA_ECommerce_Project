// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $('#logoutButton').click(function () {
        $.ajax({
            type: "POST",
            url: "/Account/Logout",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Logged out',
                        text: response.message,
                        showConfirmButton: false,
                        timer: 1500
                    }).then(function () {
                        window.location.href = '/'; // Logout sonrası ana sayfaya yönlendirme
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Failed to log out. Please try again.',
                        showConfirmButton: true
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred during the logout process.',
                    showConfirmButton: true
                });
            }
            //Testhesap.1
        });
    });
});
