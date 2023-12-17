const body = document.querySelector('.body');

$('.check-page__btn--accept').on('click', () => {
    let confirm = true;

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.addEventListener("readystatechange", function () {
        if (this.readyState === 4) {
            console.log(this.responseText)
        }
    });

    xhr.open("GET", `${domain}/Home/AgeCheck?confirm=true`);

    xhr.send();
})

$('.check-page__btn--cancel').on('click', () => {
    let confirm = false;
    
    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.addEventListener("readystatechange", function () {
        if (this.readyState === 4) {
            
        }
    });

    xhr.open("GET", `${domain}/Home/AgeCheck?confirm=false`);

    xhr.send();
})

let checkScroll = ()=>{
if ($('.check-page__bgc').length) {
$('body').css('overflow', 'hidden')
} else {
$('body').css('overflow-y', 'scroll')
}
}
$('.check-page__btn--accept').on('click', ()=>{
$('.check-page__bgc').remove()
checkScroll()
})
checkScroll()