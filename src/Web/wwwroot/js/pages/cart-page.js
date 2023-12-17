let checkTotalPrice = () => {
    let totalPrice = 0
if (!$('.cart__part-item__total-price-span').length) {
$('.cart__item--end').css('display', 'block')
$('.cart-result__total-price').text(totalPrice)
} else {
$('.cart__part-item__total-price-span').each((ind, i) => {
totalPrice += +$(i).text()
$('.cart-result__total-price').text(totalPrice)
$('.cart__item--end').css('display', 'none')
});
}
}
checkTotalPrice()
$('.cart__part-input').on('change', (e) => {
    let index = $('.cart__part-input').index($(e.currentTarget))
    let miniPrice = +$($('.cart__part-item__price-span')[index]).text()
    console.log(miniPrice);
    $($('.cart__part-item__total-price-span')[index]).text(miniPrice * +$(e.currentTarget).val())
    checkTotalPrice()
})
$('input[type="number"]').on('input', (e)=>{
if ($(e.currentTarget).val() < 1) {
$(e.currentTarget).val(1)
}
})

let deleteOrder = (ind) => {
    let id = $($('.cart__item--order')[ind]).prop('id')
    let url = `${domain}/Cart/RemoveFromCart/${id}`

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.addEventListener("readystatechange", function () {
        if (this.readyState === 4) {
            console.log(this.responseText)
        }
    });

    xhr.open("GET", `${domain}/Cart/RemoveFromCart/${id}`);

    xhr.send();

    $($('.cart__item--order')[ind]).remove()
}
$('.cart__part-item--delete').on('click', (e) => {
    $('.cart__part-item--delete').each((index, item) => {
        if (e.target == item) {
            deleteOrder(index)
        }
    })
    checkTotalPrice()
})

let setQuantity = (ind) => {
    let model = {
        id: +$($('.cart__item--order')[ind]).prop('id'),
        quantity: +$($('.cart__part-input')[ind]).val()
    }

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.addEventListener("readystatechange", function () {
        console.log(this.response)
    });

    xhr.open("POST", `${domain}/Cart/SetQuantity`);

    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.send(JSON.stringify(model));
}
$('.cart__part-input').on('change', (e) => {
    let index = $('.cart__part-input').index($(e.currentTarget))
    setQuantity(index)
})
$('.cart__part-input').each((ind, i) => {
    let miniPrice = +$($('.cart__part-item__price-span')[ind]).text()
    console.log(miniPrice);
    $($('.cart__part-item__total-price-span')[ind]).text(miniPrice * +$(i).val())
    checkTotalPrice()
})

$('.cart__part-item--delete').on('click', (e)=>{
    $('.count-cart').text(+$('.count-cart').text() - 1);
})

let checkCart = ()=>{
    let cartData = new Object
    $('.cart__item--order').each((ind, i)=>{
    cartData[$(i).attr('id')] = +$(i).find('.cart__part-input').val()
    })
    return cartData
}

let order = (e) => {
    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    let obj = {
        Data: checkCart()
    }

    xhr.addEventListener("load", () => document.write(xhr.response));

    xhr.open("POST", `${domain}/Order/Create`);

    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.send(JSON.stringify(obj));
}

$('.cart-result__btn').on('click', order)