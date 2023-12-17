
let sendData = (event) => {
     let model = {
        surname: $('input[name="surname"]').val(),
        name: $('input[name="name"]').val(),
        number: $('input[name="number"]').val()
    }

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.addEventListener("readystatechange", function () {
        if (this.readyState === 4) {
                location.href = `${domain}/Account/Im`;
        }
    });

    xhr.open("POST", `${domain}/Account/EditAccount`);

    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.send(JSON.stringify(model));
}

$('.btn__next').on('click', sendData)
