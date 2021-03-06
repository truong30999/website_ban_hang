﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using do_an_web.Models;
using do_an_web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace do_an_web.Areas.Identity.Pages.Account
{
    //[Authorize(Roles =SD.SuperAdminEndUser)]
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Chưa nhập mật khẩu.")]
            [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,12}$",
           ErrorMessage = @"Mật khẩu phải có hơn 4 ký tự,
không nhiều hơn 12 ký tự và phải bao gồm ít nhất một chữ cái viết hoa,
một chữ cái viết thường và một chữ số")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Chưa nhập xác nhận mật khẩu.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
            [Display(Name = "Confirm password")]
            public string PasswordConfirmation { get; set; }

            [Required(ErrorMessage = "Chưa nhập email")]
            [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "Email không hợp lệ.")]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Chưa nhập tên.")]
            [StringLength(255, ErrorMessage = "Tên chứa từ 2 đến 255 kí tự.", MinimumLength = 2)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Chưa nhập họ.")]
            [StringLength(255, ErrorMessage = "Họ chứa từ 2 đến 255 kí tự.", MinimumLength = 2)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }     

            [Display(Name = "Addres line ")]
            public string AddressLine { get; set; }
           
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationCustomer { UserName = Input.Email, Email = Input.Email, FirstName= Input.FirstName, LastName = Input.LastName, Address=Input.AddressLine, PhoneNumber = Input.PhoneNumber, MemberSince=DateTime.Today.Date, EmailConfirmed=true };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if(!await _roleManager.RoleExistsAsync(SD.AdminEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.AdminEndUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.SuperAdminEndUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.SuperAdminEndUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.Customer))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Customer));
                    }

                    await _userManager.AddToRoleAsync(user, SD.Customer);

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    return LocalRedirect(returnUrl);
                    //}


                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //return LocalRedirect(returnUrl);
                   return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
