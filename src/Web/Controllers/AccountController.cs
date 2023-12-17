using Azure.Core;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Web.BindingModels;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Im()
        {
            return View();
        }

        public async Task<IActionResult> EditAccount([FromBody] AccountDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                if (model.Name != null && model.Name != "")
                {
                    var claims = await _signInManager.UserManager.GetClaimsAsync(user);

                    Claim claim = null;

                    foreach (var c in claims)
                    {
                        if (c.Type == ClaimTypes.Name)
                        {
                            claim = c;
                        }
                    }

                    if (claim != null)
                    {
                        var name = new Claim(ClaimTypes.Name, model.Name);
                        await _userManager.RemoveClaimAsync(user, name);
                        await _userManager.AddClaimAsync(user, name);
                    }
                    else
                    {
                        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, model.Name));
                    }
                }

                if (model.Number != null && model.Number != "")
                {
                    await _userManager.SetPhoneNumberAsync(user, model.Number);
                }

                if (model.Surname != null && model.Surname != "" && model.Surname != "")
                {
                    var claims = await _signInManager.UserManager.GetClaimsAsync(user);

                    Claim claim = null;

                    foreach (var c in claims)
                    {
                        if (c.Type == ClaimTypes.Surname)
                        {
                            claim = c;
                        }
                    }

                    if (claim != null)
                    {
                        var surname = new Claim(ClaimTypes.Surname, model.Surname);
                        await _userManager.RemoveClaimAsync(user, surname);
                        await _userManager.AddClaimAsync(user, surname);
                    }
                    else
                    {
                        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, model.Surname));
                    }
                }
            }
            return Ok();
        }
    }
}
