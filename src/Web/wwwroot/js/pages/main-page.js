$(window).load(() => {
    $('.products-card__btn').each((ind, i) => {
        $(i).attr('href', `https://localhost:7214/Cart/AddToCart/${$(i).prop('id')}`)
    })
})

$(window).load(() => {
    $('.products-card__img').each((ind, i) => {
        $(i).attr('href', `https://localhost:7214/Catalog/Product/${$(i).prop('id')}`)
    })
})
