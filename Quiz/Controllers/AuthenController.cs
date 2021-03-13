using Quiz.Common;
using Quiz.Models;
using Quiz.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Quiz.Controllers
{
    public class AuthenController : Controller
    {
        private QuizContext db = new QuizContext();
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Session.Clear();
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public void setCookie(string username, bool rememberme = false, string role = "normal")
        {
            var authTicket = new FormsAuthenticationTicket(
                               1,
                               username,
                               DateTime.Now,
                               DateTime.Now.AddMinutes(120),
                               rememberme,
                               role
                               );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {

            if (ModelState.IsValid)
            {
                var exist = db.Users.Any(e => e.username.Equals(model.Username));
                if (exist)
                {
                    var user = db.Users.Where(e => e.username.Equals(model.Username)).First();
                    if (user != null)
                    {
                        if (user.password == Helper.CalculateMD5Hash(model.Password) && user.status == UserStatus.Activated)
                        {
                            setCookie(user.username, model.RememberMe, user.role);
                            Session["UserID"] = user.ID;
                            Session["Name"] = user.fullname;
                            if (ReturnUrl != null)
                                return Redirect(ReturnUrl);
                            return RedirectToAction("Index", "Home");
                        }
                        ModelState.AddModelError("","Sai tài khoản hoặc mật khẩu!");
                        return View();

                    }
                }

            }
            return View();
        }
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exist = db.Users.Any(e => e.username == model.username);
                if (exist)
                {
                    ModelState.AddModelError("","Tên người dùng " + model.username + " đã tồn tại");
                    return View();
                }
                exist = db.Users.Any(e => e.email == model.email);
                if (exist)
                {
                    ModelState.AddModelError("", "Email " + model.email + " đã tồn tại");
                    return View();
                }
                User u = new User
                {
                    username = model.username,
                    password = Common.Helper.CalculateMD5Hash(model.password),
                    email = model.email,
                    gender = model.gender,
                    register_date = DateTime.Now,
                    role = model.type == RegisterType.Student ? "student" : "teacher",
                    status = UserStatus.Activated,
                    fullname = model.fullname,
                    type = (UserType)model.type,
                };
                db.Users.Add(u);
                db.SaveChanges();
                FormsAuthentication.SignOut();
                setCookie(u.username, false, u.role);
                return RedirectToAction("Index");
            }
            return View();
        }
        [Authorize]
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(ChangepassViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.oldpassword == model.password)
                {
                    ModelState.AddModelError("", "Mật khẩu mới không được trùng mật khẩu cũ !");
                    return View();
                }
                else
                {
                    User user = db.Users.Where(e => e.username == User.Identity.Name).First();
                    if (user != null)
                    {
                        user.password = Common.Helper.CalculateMD5Hash(model.password);
                        db.SaveChanges();
                        return RedirectToAction("Logout");
                    }
                }
            }
            return View();
        }
    }
}
