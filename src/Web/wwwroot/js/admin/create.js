$('.form__photo-wrapper').mouseenter((e) => {
    if ($($('.form__photo-label--img')[$('.form__photo-wrapper').index(e.currentTarget)]).attr('src') !== imageUrl) {
        $(e.currentTarget).find('label').find('div').addClass('remove-photo__bg')
        $($('.remove-photo__plus')[$('.form__photo-wrapper').index(e.currentTarget)]).css('display', 'flex')
        $(e.currentTarget).mouseleave((e) => {
            $(e.currentTarget).find('label').find('div').removeClass('remove-photo__bg')
            $($('.remove-photo__plus')[$('.form__photo-wrapper').index(e.currentTarget)]).css('display', 'none')
        })
        $($('.remove-photo__plus')[$('.form__photo-wrapper').index(e.currentTarget)]).on('click', () => {
            $($('.form__photo-label--img')[$('.form__photo-wrapper').index(e.currentTarget)]).attr('src', imageUrl)
            $(e.currentTarget).find('label').find('div').removeClass('remove-photo__bg')
            $($('.remove-photo__plus')[$('.form__photo-wrapper').index(e.currentTarget)]).css('display', 'none')
        })
    }
})

let saveData = (event) => {
    $('.form__btn-submit').off('click')

    event.preventDefault()
    var data = new FormData();

    data.append("Price", $('#form').serializeObject()['Price']);
    data.append("PriceWithoutDiscount", +$('#form').serializeObject()['PriceWithoutDiscount']);

    data.append("Name", $('#form').serializeObject()['Name']);

    data.append("Weight", $('#form').serializeObject()['Weight']);
    data.append("Length", $('#form').serializeObject()['Length']);
    data.append("Width", $('#form').serializeObject()['Width']);
    data.append("Height", $('#form').serializeObject()['Height']);

    if ($($('.form__photo-label')).css('background-image')) {
        console.log(true);
    }

    data.append("Picture", $('#picture')[0].files[0]);

    let pass = true
    $(`.form__input--file`).each((index, item) => {
        if (pass) {
            pass = false
        }
        else {
            data.append("Images", $(item)[0].files[0]);
        }
    });

    if ($('#form').serializeObject()['IsTrending'] == "on") {
        data.append("IsTrending", true);
    }
    else {
        data.append("IsTrending", false);
    }

    categoriesArr.forEach((item, index, arr) => {
        data.append("Categories", item);
    });

    data.append("Manufacturer", $('#form').serializeObject()['Manufacturer']);
    data.append("VendorCode", $('#form').serializeObject()['VendorCode']);
    data.append("ValueTax", $('#form').serializeObject()['ValueTax']);
    data.append("Description", $('#form').serializeObject()['Description']);

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.addEventListener("readystatechange", function () {
        if (this.readyState === 4) {
            location.href = /*xhr.getResponseHeader("Location")*/ `${domain}/Admin/Products`;
        }
    });
   
    xhr.open("POST", requestUrl);

    xhr.send(data);
}

$('.form__btn-submit').on('click', saveData)
