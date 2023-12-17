let saveData = (event) => {
    event.preventDefault()
    var data = new FormData();

    data.append("ProductId", $('#form').serializeObject()['ProductId']);
    data.append("Price", $('#form').serializeObject()['Price']);
    data.append("PriceWithoutDiscount", $('#form').serializeObject()['PriceWithoutDiscount']);
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
        if (xhr.status == 302) {
            location.href = "https://localhost:7214/Admin/Products";
        }
    });

    xhr.open("POST", requestUrl);

    xhr.send(data);
}

$('.form__btn-submit').on('click', saveData)