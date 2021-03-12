using PagedList;
using Quiz.Models;
using Quiz.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        QuizContext _db = new QuizContext();

        [Authorize(Roles ="admin")]
        public ActionResult Index(string keyword, int page = 1, int pageSize = 7)
        {
            IPagedList<UserViewModel> users = null;
            var list = _db.Users.ToList();

            if (!String.IsNullOrEmpty(keyword))
            {
                list = list.Where(x => x.fullname.ToUpper().Contains(keyword.ToUpper())
                                    || x.username.ToUpper().Contains(keyword.ToUpper())).ToList();
            }

            users = list.Select(x => new UserViewModel()
            {
                Id = x.ID,
                Name = x.fullname,
                Email = x.email,
                UserName = x.username,
                Gender = x.gender,
                UserType = x.type,
                Status = x.status

            }).OrderByDescending(x => x.Id).ToPagedList(page, pageSize);

            ViewBag.keyword = keyword;

            ViewBag.Count = list.Count(); ;

            return View(users);
        }
        [Authorize(Roles = "admin")]
        public ActionResult Change(int id)
        {
            var user = _db.Users.Find(id);

            if (user.status == UserStatus.Activated)
            {
                user.status = UserStatus.NotActivated;
            }
            else
            {
                user.status = UserStatus.Activated;
            }

            _db.SaveChanges();

            return Json(user.fullname, JsonRequestBehavior.AllowGet);
        }
    }
}