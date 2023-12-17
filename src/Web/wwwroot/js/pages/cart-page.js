let checkTotalPrice = () => {
    let totalPrice = 0
    $('.cart__part-item__total-price-span').each((ind, i) => {
        totalPrice += +$(i).text()
        $('.cart-result__total-price').text(totalPrice)
    });
}
checkTotalPrice()
$('.cart__part-input').on('change', (e) => {
    let index = $('.cart__part-input').index($(e.currentTarget))
    let miniPrice = +$($('.cart__part-item__price-span')[index]).text()
    console.log(miniPrice);
    $($('.cart__part-item__total-price-span')[index]).text(miniPrice * +$(e.currentTarget).val())
    checkTotalPrice()
})

let domain = "https://localhost:7214";

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