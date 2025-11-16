// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

loginForm.addEventListener("submit", async function (e) {
    e.preventDefault();
    const formData = new FormData(loginForm);
    const action = loginForm.getAttribute("action") || window.location.href;

    try {
        const response = await fetch(action, {
            method: "POST",
            body: formData,
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });

        const data = await response.json();
        if (data.success) {
            modal.hide();
            showCenteredToast("Zalogowano pomyślnie!");
            window.location.reload();
        } else {
            modalBody.innerHTML = await response.text();
            setupLoginForm();
        }
    } catch (err) {
        showCenteredToast("Błąd logowania", "danger");
        console.error(err);
    }
});
