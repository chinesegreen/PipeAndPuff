$('.form__photo-wrapper').mouseenter((e)=>{
  if ($('.form__photo-wrapper')) {
    if ($($('.form__photo-label--img')[$('.form__photo-wrapper').index(e.currentTarget)]).attr('src') !== imageUrl) {
      $(e.currentTarget).find('label').find('div').addClass('remove-photo__bg')
      $($('.remove-photo__plus')[$('.form__photo-wrapper').index(e.currentTarget)]).addClass('flex')
      $(e.currentTarget).mouseleave((e)=>{
        $(e.currentTarget).find('label').find('div').removeClass('remove-photo__bg')
        $($('.remove-photo__plus')[$('.form__photo-wrapper').index(e.currentTarget)]).removeClass('flex')
      })
      $($('.remove-photo__plus')[$('.form__photo-wrapper').index(e.currentTarget)]).on('click', ()=>{
        $($('.form__photo-label--img')[$('.form__photo-wrapper').index(e.currentTarget)]).attr('src', imageUrl)
        $(e.currentTarget).find('label').find('div').removeClass('remove-photo__bg')
        $($('.remove-photo__plus')[$('.form__photo-wrapper').index(e.currentTarget)]).css('display', 'none')
      })
    }
  }
})

let editUrl = `${domain}/Admin/Product/Edit`
let editData = (event) => {
    event.preventDefault()
    var data = new FormData();

    data.append("Id", +$('.main').attr('id'));
    data.append("Price", $('#form').serializeObject()['Price']);
    data.append("PriceWithoutDiscount", +$('#form').serializeObject()['PriceWithoutDiscount']);

    data.append("Name", $('#form').serializeObject()['Name']);

    data.append("Weight", $('#form').serializeObject()['Weight']);
    data.append("Length", $('#form').serializeObject()['Length']);
    data.append("Width", $('#form').serializeObject()['Width']);
    data.append("Height", $('#form').serializeObject()['Height']);

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
            location.href = `${domain}/Admin/Products`;
        }
    });

    xhr.open("POST", editUrl);

    xhr.send(data);
}

let editImage = (event) => {
    event.preventDefault()
    var data = new FormData();

    data.append("Id", +$('.main').attr('id'));
    data.append("Position", $('.form__input--file').index($(event.currentTarget)))
    data.append("Replacer", $(event.currentTarget)[0].files[0])

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.open("POST", `${domain}/Admin/Product/EditImage`);

    xhr.send(data);
}

let deleteImage = (event) => {
    event.preventDefault()
    var data = new FormData();

    data.append("Id", +$('.main').attr('id'));
    data.append("Position", $('.remove-photo__plus').index($(event.currentTarget)))

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.open("POST", `${domain}/Admin/Product/DeleteImage`);

    xhr.send(data);
}

let addImage = (event) => {
    event.preventDefault()
    var data = new FormData();

    data.append("Id", +$('.main').attr('id'));
    data.append("Image", $(event.currentTarget)[0].files[0])

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.open("POST", `${domain}/Admin/Product/AddImage`);

    xhr.send(data);
}

$('.form__btn-edit--data').on('click', editData)

$('.remove-photo__plus').on('click', deleteImage)

$('.form__input--file').on('change', (e)=>{
  if ($($('.form__photo-label--img')[$('.form__input--file').index(e.currentTarget)]).attr('src') !== "/img/image.png") {
    editImage(e)
  } else {
    addImage(e)
  }
})

let setQuantity = (event) => {
    event.preventDefault()
    var data = {
        Id: +$('.main').attr('id'),
        Quantity: $('#form').serializeObject()['Quantity']
    }

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.open("POST", `${domain}/Admin/Product/SetQuantity`);

    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.send(JSON.stringify(data));
}

$('.form__btn-send').on('click', setQuantity)