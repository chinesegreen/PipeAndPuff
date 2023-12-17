let filters = {
  category:'',
  size:'',
  color:'',
  brad:''
}
let colors = ['#E1523D', '#32127A', '#F8D568', '#478430', '#6DAE81']
colors.forEach((item, index, arr) => {
  $($('.filters__item-child--colors')[index]).css('background-color', item)
});
// $('.filters__item-child--colors').on('click', (e)=>{
//   filters.color = $(e.currentTarget).css('background-color')
//   console.log(filters);
// })
$('.filters__item-title').on('click', (e)=>{
  $(e.currentTarget).next().toggleClass('filters__item-child--active')
})
$('.filters__item-child--colors-input').on('change', (e)=>{
  $('.filters__item-child--colors').css('transform', 'scale(1)')
  let index = $('.filters__item-child--colors-input').index($(e.currentTarget))
  $($('.filters__item-child--colors')[index]).css('transform', 'scale(0.9)')
})