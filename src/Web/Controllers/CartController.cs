using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Web.Configuration;
using Web.ViewModels;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Authorization;
using Core.Entities.OrderAggregate;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Web.BindingModels;

namespace Web.Controllers
{
    public class CartController : BaseController
    {
        const string SessionKey = "_Cart";
        private readonly CatalogContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(CatalogContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("[Controller]")]
        public IActionResult Cart(string returnUrl)
        {
            return View(new CartViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        public IActionResult AddToCart(int id)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                var cart = GetCart();
                cart.AddItem(product, 1);
                HttpContext.Session.Set<Cart>(SessionKey, cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SetQuantity([FromBody] SetQuantityCommand cmd)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.Id == cmd.Id);
            
            if (product != null)
            {
                var cart = GetCart();
                cart.SetQuantity(product, cmd.Quantity);
                HttpContext.Session.Set<Cart>(SessionKey, cart);
            }

            return Ok();
        }

        public IActionResult RemoveFromCart(int id)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveLine(product);
                HttpContext.Session.Set<Cart>(SessionKey, cart);
            }

            return Ok();
        }

        public static Cart GetCart(HttpContext context)
        {
            Cart? cart = context.Session.Get<Cart>(SessionKey);

            if (cart == null)
            {
                cart = new Cart();
                context.Session.Set<Cart>(SessionKey, cart);
            }

            return cart;
        }

        public Cart GetCart()
        {
            Cart? cart = HttpContext.Session.Get<Cart>(SessionKey);

            if (cart == null)
            {
                cart = new Cart();
                HttpContext.Session.Set<Cart>(SessionKey, cart);
            }

            return cart;
        }

        [Authorize]
        [HttpGet("chbsnc")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand cmd)
        {
            string buyerId = _userManager.GetUserId(User)!;
            Address shipToAddress = cmd.Address;
            List<OrderItem> items = new List<OrderItem>();

            var cart = GetCart();
            
            foreach (var line in cart.Lines)
            {
                var product = line.Product;

                var catalogItemOrdered = new CatalogItemOrdered(product.Id, product.Name, product.Picture);
                var orderItem = new OrderItem(catalogItemOrdered,line.Product.Price, line.Quantity);

                items.Add(orderItem);
            }

            var order = new Order(buyerId, shipToAddress, items);

            _context.Add(order);

            await _context.SaveChangesAsync();

            return View();
        }

        public IActionResult Purchase()
        {
            return View();
        }
    }
}
