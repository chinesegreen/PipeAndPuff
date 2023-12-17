$('.header__search-inp').keydown(function (e) {
    if (e.keyCode === 13) {
        window.location.href = `https://localhost:7214/Catalog/Search/${$(e.currentTarget).val()}`
    }
})
