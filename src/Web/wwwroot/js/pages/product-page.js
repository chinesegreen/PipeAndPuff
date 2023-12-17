$('.preview-wrapper__img').click((e)=>{
  $('.imgs-wrapper__show-wrapper').css('background-image', `url("${$(e.currentTarget).attr('src')}")`)
  $('.show-wrapper__img').attr('src', $(e.currentTarget).attr('src'))
})

$('input[type="number"]').on('input', (e)=>{
if ($(e.currentTarget).val() < 1) {
$(e.currentTarget).val(1)
}
})

let goto = (event) => {
    event.preventDefault
    var xhr = new XMLHttpRequest()
    xhr.withCredentials = true

    let cmd = {
        "Categories": []
    }
    
    cmd.Categories.push($(event.currentTarget).text())

    xhr.open("POST", `${domain}/Catalog/SetFilters`)

    xhr.addEventListener("readystatechange", function () {
    if (this.readyState === 4) {
        location.href = `${domain}/Catalog`;
        }
    });

    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.send(JSON.stringify(cmd));
}

$('.category__link').on('click', goto)
