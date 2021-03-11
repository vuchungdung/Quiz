using Quiz.Common;
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
    public class QuizTestController : Controller
    {
        QuizContext _db = new QuizContext();

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "admin,teacher")]
        public ActionResult Create()
        {
            var viewModel = new QuizTestViewModel()
            {
                Subject = Helper.getSubjectItem()
            };
            return View(viewModel);
        }
        [Authorize(Roles = "admin,teacher")]
        public ActionResult Update()
        {
            return View();
        }
        public JsonResult SearchQuiz(int subject, string name = null, HardType? hard = null)
        {
            try
            {
                List<QuizSearchViewModel> lst = null;
                if (String.IsNullOrWhiteSpace(name))
                {
                    if (hard.HasValue)
                    {
                        lst = _db.Quizzes.Where(i => i.SubjectID == subject && i.HardType == hard).Take(30).Select(a => new QuizSearchViewModel
                        {
                            HardType = a.HardType,
                            id = a.QuizID,
                            Name = a.name,
                        }).ToList();
                    }
                    else
                    {
                        lst = _db.Quizzes.Where(i => i.SubjectID == subject).Take(30).Select(a => new QuizSearchViewModel
                        {
                            HardType = a.HardType,
                            id = a.QuizID,
                            Name = a.name,
                        }).ToList();
                    }
                }
                else
                {
                    if (hard.HasValue)
                    {
                        lst = _db.Quizzes.Where(i => i.SubjectID == subject && i.HardType == hard && i.name.Contains(name)).Take(30).Select(a => new QuizSearchViewModel
                        {
                            HardType = a.HardType,
                            id = a.QuizID,
                            Name = a.name,
                        }).ToList();
                    }
                    else
                    {
                        lst = _db.Quizzes.Where(i => i.SubjectID == subject && i.name.Contains(name)).Take(30).Select(a => new QuizSearchViewModel
                        {
                            HardType = a.HardType,
                            id = a.QuizID,
                            Name = a.name,
                        }).ToList();
                    }
                }

                return Json(new { QuizList = lst }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                return Json(new { Message = "Lỗi hệ thống" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin,teacher")]
        public JsonResult Create(QuizTestViewModel model)
        {
            model.Subject = Common.Helper.getSubjectItem();
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, Message = "Bạn nhập thiếu các trường yêu cầu" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                User u = _db.Users.Where(i => i.username == User.Identity.Name).First();

                QuizTest test = new QuizTest
                {
                    CreatorID = u.ID,
                    Creator = u,
                    CreateDate = DateTime.Now,
                    SubjectID = model.SubjectID,
                    TotalMark = model.TotalMark,
                    name = model.name,
                    TotalTime = (int)model.TotalTime,
                    status = (TestStatusAd)model.status,
                };
                foreach (var item in model.quizID)
                {
                    Quizs q = _db.Quizzes.Find(item);
                    test.Quiz.Add(q);
                }
                _db.QuizTests.Add(test);
                _db.SaveChanges();
                return Json(new { success = true, Message = "Tạo đề thi thành công !" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}