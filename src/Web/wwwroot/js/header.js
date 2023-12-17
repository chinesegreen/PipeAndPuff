$('.header__search-inp').keydown(function (e) {
    if (e.keyCode === 13) {
        window.location.href = `${domain}/Catalog/Search/${$(e.currentTarget).val()}`
    }
})
