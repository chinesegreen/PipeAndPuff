let filters = {
Categories:[],
MinPrice:0,
MaxPrice: 100000,
Brands:[]
}
$('.filters__item-title').on('click', (e)=>{
$(e.currentTarget).next().toggleClass('filters__item-child--active')
})

function deleteItem(arr, to, setting) {
if (arr.length > 0) {
for (let i = 0, len = arr.length; i < len; i++) {
if (arr[i] === to) {
arr.splice(i, 1);
break;
}
}
} else if (arr.length == 0) {
filters[setting].pop();
}
}
let changeItem = (e, setting)=>{
if ($(e.target).prop('checked')) {
$(e.target).prop('checked', true)
filters[setting].push($(e.target).val())
} else {
$(e.target).prop('checked', false)
deleteItem(filters[setting], $(e.target).val(), setting)
}
return filters[setting]
}
$('.filters__item--category').on('change', (e)=>{
changeItem(e, 'Categories')
})
$('.filters__item--manufacture').on('change', (e)=>{
changeItem(e, 'Brands')
})

$('.filters__item--price').on('change',(e)=>{
if ($(e.currentTarget).prop('id') == 'priceFrom') return filters.MinPrice = +$(e.currentTarget).val()
filters.MaxPrice = +$(e.currentTarget).val()
})

function filterItems(e, place){
const searchedText = e.target.value.toLowerCase()


place.each((index, item) => {
const itemText = item.textContent.toLowerCase()
if(itemText.includes(searchedText)){
item.style.display = "block"
} else{
item.style.display = "none"
}
})
}

$('input[type="number"]').on('input', (e)=>{
if ($(e.currentTarget).val() < 1) {
$(e.currentTarget).val(1)
}
})

$('.search-category').on('input', (e)=>{
filterItems(e, $('.filters__item--category'))
})
$('.search-brand').on('input', (e)=>{
filterItems(e, $('.filters__item--manufacture'))
})

let sendFilters = (event) => {
    event.preventDefault()

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    console.log(filters)

    xhr.addEventListener("readystatechange", function () {
        if (this.readyState === 4) {
            location.href = `${domain}/Catalog`;
        }
    });

    xhr.open("POST", `${domain}/Catalog/SetFilters`);

    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.send(JSON.stringify(filters));
}

$('.search').on('click', sendFilters)

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

let removeFilters = (event) => {
    event.preventDefault
    var xhr = new XMLHttpRequest()
    xhr.withCredentials = true
    
    xhr.open("GET", `${domain}/Catalog/RemoveFilters`)

    xhr.addEventListener("readystatechange", function () {
    if (this.readyState === 4) {
        location.href = `${domain}/Catalog`;
        }
    });

    xhr.send()
}

$('.filters-header__clear').on('click', removeFilters)