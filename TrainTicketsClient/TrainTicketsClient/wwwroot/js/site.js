/* Save information in form after reloading */
document.addEventListener("DOMContentLoaded", function () {

    document.querySelectorAll('input').forEach(function (e) {

        if (e.value === '') e.value = window.sessionStorage.getItem(e.name, e.value);

        e.addEventListener('input', function () {
            window.sessionStorage.setItem(e.name, e.value);
        })
    })
});

function SaveSelectValue(el) {
    window.sessionStorage.setItem(el.name, el.value);
}
function LoadSelectValue(el) {
    return window.sessionStorage.getItem(el.name);
}


/* Show and Hide Update Section */
const btns = document.querySelectorAll(".btn-update");
const contents = document.querySelectorAll(".update-section");

btns.forEach(function (btn) {
    btn.addEventListener('click', function (e) {       
        let i = Math.floor($(this).parent().parent().parent().index() / 2);
        contents[i].classList.toggle("hidden");
    })
})
