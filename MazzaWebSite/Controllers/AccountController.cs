using Google.Authenticator;
using MazzaWebSite.Identity;
using MazzaWebSite.Models;
using MazzaWebSite.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MazzaWebSite.Models.Enums;

namespace MazzaWebSite.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ISendEmail _notificationService = null;
        private ICookie _cookie = null;
        TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        public AccountController()
        {
            _notificationService = new SendEmail();
            _cookie = new Cookie();
            
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index()
        {
            using (var dbContext = new MazzaDbContext())
            {
                var userId = User.Identity.GetUserId<int>();
                var UserGA = dbContext.GoogleAuths.FirstOrDefault(g => g.UserId == userId);
                string qrCodeImageUrl = string.Empty;
                string manualEntryKey = string.Empty;
                if (UserGA == null)
                {
                    var setupInfo = tfa.GenerateSetupCode(General.OMPTitle, dbContext.Users.FirstOrDefault(u => u.Id == userId).Email, "SuperSecretKeyGoesHere", 200, 200, true);

                    qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                    manualEntryKey = setupInfo.ManualEntryKey;
                    dbContext.GoogleAuths.Add(new GoogleAuthentication
                    {
                        UserId = userId,
                        QrCodeUrl = qrCodeImageUrl,
                        ManualEntryKey = manualEntryKey,
                        AccountSecretKey = "SuperSecretKeyGoesHere",
                        IsActive = false,
                        CreatedOn = DateTime.UtcNow
                    });
                    dbContext.SaveChanges();
                }
                else
                {
                    qrCodeImageUrl = UserGA.QrCodeUrl;
                    manualEntryKey = UserGA.ManualEntryKey;
                }
                ManageViewModel model = new ManageViewModel
                {
                    Users = dbContext.Users.ToList(),
                    QrCodeImageUrl = qrCodeImageUrl,
                    ManualEntryKey = manualEntryKey,
                    IsActive = UserGA != null ? UserGA.IsActive : false
                };

                return View(model);
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Prova()
        {

            return Json(new { success = true, Status = "success", Message = "Cazzo tette culo figa" });
        }

        public ActionResult Google2Auth(string token)
        {
            using (var dbContext = new MazzaDbContext())
            {
                var userId = User.Identity.GetUserId<int>();
                var userGA = dbContext.GoogleAuths.FirstOrDefault(g => g.UserId == userId);

                string message = string.Empty;
                string status = string.Empty;
                if (!userGA.IsActive)
                {
                    var validate = tfa.ValidateTwoFactorPIN(userGA.AccountSecretKey, token, TimeSpan.FromSeconds(5));
                    if (validate)
                    {
                        userGA.IsActive = true;
                        dbContext.SaveChanges();
                        status = Success;
                        message = "Change with success";
                    }
                    else
                    {
                        status = Danger;
                        message = "Error";
                    }
                }
                return Json(new { success = true, Status = status, Message = message });
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


        [AllowAnonymous]
        public ActionResult Register(string @ref)
        {
            ViewBag.CountryList = GetCountrylist();
            CheckReferentCookie(@ref);
            _cookie.CheckReferentCookie(Request, Response, @ref);
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            User referent;
            using (var dbContext = new MazzaDbContext())
            {
                referent = dbContext.Users.FirstOrDefault(u => u.UserName.Equals(model.ReferentCode));
            }
            try
            {
                if (referent != null && Request.Cookies["OMP-referentcode"].Value.Equals(model.ReferentCode))
                {
                    if (ModelState.IsValid)
                    {
                        var PasswordHash = new PasswordHasher();

                        var newUser = new ApplicationUser
                        {
                            ReferentId = referent.Id,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            UserName = model.UserName,
                            Gender = model.Gender,
                            CountryId = model.CountryId,
                            DateOfBirth = model.DateOfBirth,
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                            PasswordHash = PasswordHash.HashPassword(model.Password),
                            RegisterOn = DateTime.UtcNow
                        };

                        var result = await UserManager.CreateAsync(newUser, model.Password);
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(newUser, isPersistent: false, rememberBrowser: false);


                            var emailEntity = new EmailEntity
                            {
                                Referent = referent,
                                Affiliate = newUser,
                                Culture = _cookie.GetCookieLanguage(Request, Response).Value
                            };

                            _notificationService.SendEmailFromTemplate(NotificationTemplateTypes.UserRegistration, emailEntity);

                            _notificationService.SendEmailFromTemplate(NotificationTemplateTypes.NewAffiliateRegistration, emailEntity);

                            return RedirectToAction("Index", "Home");

                        }
                        AddErrors(result);

                    }
                }
                else
                {
                    ModelState.AddModelError("", Account.UserNotFound);
                    ViewBag.CountryList = GetCountrylist();
                }
                ViewBag.CountryList = GetCountrylist();
            }
            catch (Exception ex)
            {
                SendEmail.Send("lmazzaferro6@gmail.com", "Errore Register", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ManageViewModel model)
        {
            string message = null;
            string status = null;

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.Where(m => m.Errors.Count > 0))
                    message += string.Concat(error.Errors[0].ErrorMessage, "\r\n");
            }
            else
            {
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    status = Success;
                    message = "Password cambiata con successo";
                }
            }
            return Json(new { success = true, Status = status ?? Danger, Message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePaymentMethod(ManageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ViewBag.UserNotFound = true;
                    return View();
                }
                
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme);

                SendEmail.Send(user.Email, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                var emailEntity = new EmailEntity
                {
                    Affiliate=user,
                    CallbackUrl= callbackUrl,
                    Culture = _cookie.GetCookieLanguage(Request,Response).Value
                };
                _notificationService.SendEmailFromTemplate(NotificationTemplateTypes.ForgotPassword, emailEntity);
                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Index", "Home");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            AddErrors(result);
            return View();
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        private List<SelectListItem> GetCountrylist()
        {
            using (var dbContext = new MazzaDbContext())
            {
                var dbContextValues = dbContext.Countries.ToList();

                var countries = new SelectList(dbContextValues.Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }).ToList().OrderBy(c => c.Text), "Value", "Text");

                return countries.ToList();
            }
        }

        public void CheckReferentCookie(string @ref)
        {
            ViewBag.ReferentCode = @ref;
            HttpCookie cookie = Request.Cookies["OMP-referentcode"];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                cookie = new HttpCookie("OMP-referentcode")
                {
                    Expires = DateTime.Now.AddMonths(3),
                    Value = @ref ?? string.Empty
                };
                HttpContext.Response.Cookies.Add(cookie);
            }
        }
        #endregion

    }
}