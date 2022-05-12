using E_ccomerce_Task2.Data;
using E_ccomerce_Task2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace E_ccomerce_Task2.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private ApplicationDbContext context;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // =========================Sign up===============================================

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {

                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName=model.FirstName,
                    LastName=model.LastName,
                    City=model.City,
                    Country=model.Country,
                    Address1=model.Address1,
                    Address2=model.Address2,
                    ZipCode=model.ZipCode
                   
                };

               var result=await userManager.CreateAsync(user, model.Password);
              
                if (result.Succeeded)
                {
                    var result1 =await userManager.AddToRoleAsync(user,"Customer");
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "OnlineStore");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
          

            return View(model);
            // 
        }

    /*===================LogIn ====================================*/
    
       
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                string s = "";
                s += result.Succeeded + " result \n";
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "OnlineStore");
                }

                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();
                

                foreach (var e in errors)
                {
                    s += e.ToString();
                    s += "\n";
                    
                }
                s += model.Email + " Email\n";
                s += model.Password + " pass\n";
                s += model.RememberMe+" remember";
              
          
                ModelState.AddModelError("", s);
            }
          
            return View(model);

        }

        /*=================== Forgot passward ====================================*/
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            /*"$TarsMoon2022"*/
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var callback = Url.Action(nameof(resetPassword), "Account", new { token, email = user.Email }, Request.Scheme);


                    sendEmail("Reset Password",
                            "Please confirm your account by clicking this link: <a href=\""
                                               + callback + "\">link</a>", true,
                            "salsabeeldaraghmeh75@gmail.com");
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));



                }
            }
            return View(model);
        }
        public ActionResult ForgotPasswordConfirmation() 
        {  
            return View();

        }

        [HttpGet]
        public IActionResult resetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> resetPassword(ResetPassword resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);
            var user = await userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.NewPassword);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        /*====================== My Account =========================*/
        [Authorize]
        public ActionResult myAccount()
        {
            return View();
        }
        /*===================== Account setting ======================*/

            /*======= Reset account Password ==============*/
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> resetAccountPassword()
        {
            var user = await userManager.GetUserAsync(User);
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            
            var model = new ResetPassword { Token = token, Email = user.Email };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> resetAccountPassword(ResetPassword resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);
            var user = await userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.NewPassword);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }

            return RedirectToAction(nameof(myAccount));
        }


            /*=======Profile setting General Section ==============*/

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> editGeneralInfo() 
        {
            var user = await userManager.GetUserAsync(User);
            var profile = new GeneralInfo
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email=user.Email
            };
            return View(profile);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> editGeneralInfo(GeneralInfo info)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                user.FirstName = info.FirstName;
                user.LastName = info.LastName;
                var result =await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(myAccount));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

            }
            return View(info);
        }

        /*=======Address setting Section ==============*/

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> editAddressInfo()
        {
            var user = await userManager.GetUserAsync(User);
            var addressInfo = new AddressInfo
            {
                Address1 = user.Address1,
                Address2 = user.Address2,
                City = user.City,
                Country=user.Country,
                ZipCode= user.ZipCode
                
            };
            return View(addressInfo);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> editAddressInfo(AddressInfo info)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                user.Address1 = info.Address1;
                user.Address2=info.Address2;
                user.City = info.City;
                user.Country = info.Country;
                user.ZipCode = info.ZipCode;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(myAccount));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

            }
            return View(info);
        }


        /*=======================* Helping functions =================*/

        public void sendEmail(String subject,string body,bool isBodyHtml,String toEmail)
        {
            using (MailMessage message = new MailMessage("starsmoononlinestore@gmail.com", toEmail))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isBodyHtml;
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.Host = ("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    NetworkCredential cred = new NetworkCredential("starsmoononlinestore@gmail.com", "$TarsMoon2022");
                    smtpClient.Credentials = cred;
                    smtpClient.UseDefaultCredentials = false;


                    smtpClient.Send(message);
                }
            }

        }
    }



}


/*
   var errors = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new { x.Key, x.Value.Errors })
            .ToArray();
            string s = "";
            foreach (var e in errors)
            {
                s += e.ToString();
                s += "\n";
            }
            s += "id = " + usercourse.UserId;
            Response.Write(s);
 */
