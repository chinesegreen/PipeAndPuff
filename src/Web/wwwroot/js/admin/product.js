window.addEventListener('load', (e) => {
    e.preventDefault();
});
const requestUrl = 'https://localhost:7214/Admin/Product/Create'
$('.main-nav__link').each((indLink, iLink) => {
    $(iLink).on('click', (e) => {
        $('.form__view').each((indSection, iSection) => {
            if (indLink == indSection) {
                $(iSection).removeClass('main__form--none')
                $('.main-nav__link').addClass('main-nav__link--disable')
                $(iLink).removeClass('main-nav__link--disable')
                $('.main-nav__span').each((indSpan, iSpan) => {
                    if (indSpan == indSection) {
                        $(iSpan).removeClass('main-nav__span--disable')
                    } else {
                        $(iSpan).addClass('main-nav__span--disable')
                    }
                })
            } else {
                $(iSection).addClass('main__form--none')
            }
        })
    })
})

let sendRequest = (method, url, body = null) => {
    const headers = {
        'Content-Type': 'application/json'
    }
    return fetch(url, {
        method: method,
        body: JSON.stringify(body),
        headers: headers
    }).then(response => {
        if (response.ok) {
            return response.json()
        }
    })
}
$.fn.serializeObject = function () {
    let o = {}
    let a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]]
            }
            o[this.name].push(this.value || '')
        } else {
            o[this.name] = this.value || ''
        }
    });
    return o;
}
$('.form__btn-preview').on('click', () => {
    $('.form__view').addClass('main__form--none')
    $($('.form__view')[$('.form__view').length - 1]).removeClass('main__form--none')
    $('.main-nav__link').addClass('main-nav__link--disable')
    $('.main-nav__span').addClass('main-nav__span--disable')
    $($('.main-nav__link')[$('.main-nav__link').length - 1]).removeClass('main-nav__link--disable')
    $($('.main-nav__span')[$('.main-nav__link').length - 1]).removeClass('main-nav__span--disable')

})

$('.form__input--select').keydown(function (e) {
    if (e.keyCode === 13) {
        if ($(`.form__input--select`).val() !== '') {
            categoriesArr.push(`${$(`.form__input--select`).val()}`)

            let newItem = document.createElement('li')
            $(newItem).addClass('categories__item')
            $(newItem).append(categoriesArr[categoriesArr.length - 1])
            $('.categories__list').append(newItem)
            categoriesArr.push(`${$(`.form__input--select`).val()}`)
        }
        $('.categories__item').on('click', deleteCategories)
        $('.form__input--select').val('')
    }
})

let photoOrd = []
function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();
        reader.onload = function (e) {
            if (photoOrd.length < 15) {
                $($('.form__photo-label')[photoOrd.length]).css('background-image', `url(${e.target.result})`)
                $($('.form__photo-label')[photoOrd.length + 15]).css('background-image', `url(${e.target.result})`)
                $('.form__title--numcount').text(photoOrd.length + 1)
                photoOrd.push($('.form__input--file').val())
            } else {
                $('.form__title--count').css('color', '#ff0000')
            }
        }
        reader.readAsDataURL(input.files[0]);
    }
}

$(document).ready(function () {
    $('.form__input--file').on('change', () => {
        if ($('.form__title--numcount').text() < 15) {
            let countFile = 0
            $('.form__title--numcount').text(countFile)
            $('.form__input--file').each((ind, i) => {
                if ($(i).val() !== '') {
                    countFile++
                    $('.form__title--numcount').text(countFile)
                }
            })
        } else {
            $('.form__title--count').css('color', '#ff0000')
        }
    })
})

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();
        reader.onloadend = function (e) {
            $($('.form__photo-label--img')[$('.form__input--file').index(input)]).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}
$('.form__input--file').on('change', (e) => {
    readURL(e.currentTarget)
})

let categoriesArr = []
let deleteCategories = (e)=>{
if (categoriesArr.length > 0) {
for (let i = 0, len = categoriesArr.length; i < len; i++) {
if (categoriesArr[i] == $(e.currentTarget).text()) {
categoriesArr.splice(i, 1);
$(e.currentTarget).remove()
break;
}
}
}
}
$('.form__categories--btn').on('click', (e)=>{
let bool = categoriesArr.indexOf($(`.form__input--select`).val()) !== -1
if (!bool) {
categoriesArr.push(`${$(`.form__input--select`).val()}`)
let newItem = document.createElement('li')
$(newItem).addClass('categories__item')
$(newItem).append(categoriesArr[categoriesArr.length - 1])
$('.categories__list').append(newItem)
}
$('.categories__item').on('click', deleteCategories)
$('.form__input--select').val('')
})
