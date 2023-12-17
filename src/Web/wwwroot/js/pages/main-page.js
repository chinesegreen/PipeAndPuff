$(window).load(() => {
    $('.products-card__btn').each((ind, i) => {
        $(i).attr('href', `${domain}/Cart/AddToCart/${$(i).prop('id')}`)
    })
})

$(window).load(() => {
    $('.products-card__img').each((ind, i) => {
        $(i).attr('href', `${domain}/Catalog/Product/${$(i).prop('id')}`)
    })
})

$('.load-more-product').on('click', (e)=>{
    location.href = `${domain}/Catalog`;
})

$('.product__btn').on('click', (e)=>{
    if ($(e.currentTarget).hasClass('product__btn--active')) {
        window.location.href = `${domain}/Cart`;
    }
    else {
        $('.count-cart').text(+$('.count-cart').text() + 1);
        e.preventDefault
        var xhr = new XMLHttpRequest()
        xhr.withCredentials = true
    
        xhr.open("GET", `${domain}/Cart/AddToCart/${$(e.currentTarget).prop('id')}`)

        xhr.send()

        $(e.currentTarget).text('TO CART')
        $(e.currentTarget).addClass('product__btn--active')
    }
})