$.fn.checkForm = function() {
  let obj = new Object
  $(this).find('input').each((ind, i)=>{
    if (!obj.hasOwnProperty($(i).attr('data-type'))) {
      obj[$(i).attr('data-type')] = new Array
    }
    if ($(i).val() == '') {
      obj[$(i).attr('data-type')].push('')
    } else if ($(i).attr('type') == 'file') {
      obj[$(i).attr('data-type')].push($(i)[0].files[0])
    } else {
      obj[$(i).attr('data-type')].push($(i).val())
    }
  })
  return obj
}

let url = `${domain}/Showcase/Edit`

let sendShowcaseData = (event) => {
    event.preventDefault()
    var data = new FormData();

    $($('#form__showcase').checkForm()['files']).each((index, item) => {
        data.append(`Image${index+1}`, item)
    });

    $($('#form__showcase').checkForm()['subtitles']).each((index, item) => {
        data.append("Subtitles", item)
    });

    $($('#form__showcase').checkForm()['titles']).each((index, item) => {
        data.append("Titles", item)
    });

    $($('#form__showcase').checkForm()['links']).each((index, item) => {
        data.append("Links", item)
    });

    $($('#form__category').checkForm()['files']).each((index, item) => {
        data.append(`CImage${index+1}`, item)
    });

    $($('#form__category').checkForm()['subtitles']).each((index, item) => {
        data.append("CSubtitles", item)
    });

    $($('#form__category').checkForm()['titles']).each((index, item) => {
        data.append("CTitles", item)
    });

    $($('#form__category').checkForm()['links']).each((index, item) => {
        data.append("CLinks", item)
    });

    var xhr = new XMLHttpRequest();
    xhr.withCredentials = true;

    xhr.addEventListener("readystatechange", function () {
        if (this.readyState === 4) {
            location.href = `${domain}/Admin/Products`;
        }
    });

    xhr.open("POST", url);

    xhr.send(data);
}

$('.btn__next').on('click', sendShowcaseData)

console.log($('#form__showcase').checkForm());
console.log($('#form__category').checkForm());