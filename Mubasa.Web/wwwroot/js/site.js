// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function confirmDelete(title, message, yesOpt, noOpt, id, path) {
    Notiflix.Confirm.show(
        title,
        message,
        yesOpt,
        noOpt,
        function yCb() {
            yesCb(id, path);
        },
        function nCb() {
            
        },
        {
            width: '320px',
            borderRadius: '5px',
            okButtonBackground: 'red'
        },
    );
}

async function yesCb(id, path) {
    host = window.location.host;
    await fetch(`https://${host}/${path}/${id}`, { method: "DELETE" })
        .then(response => {
            return response.json();
        })
        .then(data => {
            if (data.success) {
                Notiflix.Notify.success(data.message);
                setTimeout(() => {
                    window.location.reload();
                }, 300)
            } else {
                Notiflix.Notify.failure(data.message);
            }
        });
}

window.onload = function () {
    const loader = document.getElementById("loader");
    const main = document.getElementById("main");
    loader.style.display = "none";
    main.style.filter = "none";
}
