using PagedList;
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
        public ActionResult Index(string keyWord, int page = 1, int pageSize = 7)
        {
            IPagedList<QuizTestViewModel> quiztests = null;
            var list = _db.QuizTests.ToList();

            if (!String.IsNullOrEmpty(keyWord))
            {
                list = list.Where(x => x.name.ToUpper().Contains(keyWord.ToUpper())).ToList();
            }

            quiztests = list.Select(x => new QuizTestViewModel()
            {
                TestID = x.TestID,
                name = x.name,
                SubjectName = x.Subject.name,
                TotalTime = (TimeQuiz)x.TotalTime,
                TotalMark = x.TotalMark,
                status = (TestStatus)x.status

            }).OrderByDescending(x => x.TestID).ToPagedList(page, pageSize);

            ViewBag.keyWord = keyWord;

            ViewBag.Count = list.Count(); ;

            return View(quiztests);
        }
        [Authorize(Roles = "admin,teacher")]
        public ActionResult MyIndex(string keyWord, int page = 1, int pageSize = 7)
        {
            var userID = _db.Users.Where(t => t.username == User.Identity.Name).First().ID;
            IPagedList<QuizTestViewModel> quiztests = null;
            var list = _db.QuizTests.Where(x=>x.CreatorID == userID).ToList();

            if (!String.IsNullOrEmpty(keyWord))
            {
                list = list.Where(x => x.name.ToUpper().Contains(keyWord.ToUpper())).ToList();
            }

            quiztests = list.Select(x => new QuizTestViewModel()
            {
                TestID = x.TestID,
                name = x.name,
                SubjectName = x.Subject.name,
                TotalTime = (TimeQuiz)x.TotalTime,
                TotalMark = x.TotalMark,
                status = (TestStatus)x.status

            }).OrderByDescending(x => x.TestID).ToPagedList(page, pageSize);

            ViewBag.SearchString = keyWord;

            ViewBag.Count = list.Count(); ;

            return View(quiztests);
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
                    status = (TestStatus)model.status,
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
        [Authorize(Roles = "admin,teacher")]
        [HttpGet]
        public ActionResult Update(int id)
        {
            var CurrentID = Session["UserID"];
            try
            {
                var exist = _db.QuizTests.Any(e => e.TestID == id);
                if (!exist)
                {
                    return RedirectToAction("MyQuizTest");
                }
                QuizTest test = _db.QuizTests.Find(id);

                if ((test.CreatorID != (int)CurrentID && User.IsInRole("teacher")))
                {
                    return RedirectToAction("MyQuizTest");
                }
                QuizTestViewModel quiz = new QuizTestViewModel
                {
                    name = test.name,
                    Subject = Common.Helper.getSubjectItem(),
                    SubjectID = test.SubjectID,
                    status = (TestStatus)test.status,
                    TestID = test.TestID,
                    TotalMark = test.TotalMark,
                    TotalTime = (TimeQuiz)test.TotalTime,
                };
                ViewBag.id = test.TestID;
                return View(quiz);
            }
            catch
            {
                return RedirectToAction("MyQuizTest");
            }
        }
        public JsonResult SearchQuiz(int subject, string name = null, HardType? hard = null)
        {
            User u = _db.Users.Where(i => i.username == User.Identity.Name).First();
            try
            {
                List<QuizSearchViewModel> lst = null;
                if (String.IsNullOrWhiteSpace(name))
                {
                    if (hard.HasValue)
                    {
                        lst = _db.Quizzes.Where(i => i.SubjectID == subject && i.HardType == hard && i.CreatorID == u.ID).Take(30).Select(a => new QuizSearchViewModel
                        {
                            HardType = a.HardType,
                            id = a.QuizID,
                            Name = a.name,
                        }).ToList();
                    }
                    else
                    {
                        lst = _db.Quizzes.Where(i => i.SubjectID == subject && i.CreatorID == u.ID).Take(30).Select(a => new QuizSearchViewModel
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
                        lst = _db.Quizzes.Where(i => i.SubjectID == subject && i.HardType == hard && i.name.Contains(name) && i.CreatorID == u.ID).Take(30).Select(a => new QuizSearchViewModel
                        {
                            HardType = a.HardType,
                            id = a.QuizID,
                            Name = a.name,
                        }).ToList();
                    }
                    else
                    {
                        lst = _db.Quizzes.Where(i => i.SubjectID == subject && i.name.Contains(name) && i.CreatorID == u.ID).Take(30).Select(a => new QuizSearchViewModel
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
        public JsonResult GetQuizFromTest(int testid)
        {
            QuizTest test = _db.QuizTests.Find(testid);
            User u = _db.Users.Where(i => i.username == User.Identity.Name).First();
            List<QuizViewModel> quizzes = test.Quiz.Where(x => x.CreatorID == u.ID).Select(
                c => new QuizViewModel
                {
                    Id = c.QuizID,
                    HardType = c.HardType,
                    Name = c.name,

                }).ToList();
            return Json(quizzes);
        }
        [Authorize(Roles = "admin,teacher")]
        [HttpPost]
        public ActionResult Update(QuizTestViewModel model)
        {
            model.Subject = Common.Helper.getSubjectItem();
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, Message = "Bạn nhập thiếu các trường yêu cầu" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                User u = _db.Users.Where(i => i.username == User.Identity.Name).First();

                QuizTest test = _db.QuizTests.Find(model.TestID);
                test.CreatorID = u.ID;
                test.Creator = u;
                test.CreateDate = DateTime.Now;
                test.TotalMark = model.TotalMark;
                test.name = model.name;
                test.TotalTime = (int)model.TotalTime;
                test.status = (TestStatus)model.status;
                foreach (var item in test.Quiz.ToList())
                {
                    test.Quiz.Remove(item);                   
                }
                foreach(var quiz in model.quizID)
                {
                    var q = _db.Quizzes.Find(quiz);
                    test.Quiz.Add(q);
                }
                _db.SaveChanges();
                return Json(new { success = true, Message = "Cập đề thi thành công !" }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "admin,teacher")]
        public JsonResult Delete(int id)
        {
            try
            {
                var model = _db.QuizTests.Find(id);
                foreach(var item in model.Quiz.ToList())
                {
                    model.Quiz.Remove(item);
                }
                _db.QuizTests.Remove(model);
                _db.SaveChanges();
                return Json(model.name, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [Authorize(Roles = "student")]
        public ActionResult IndexStudent(string keyWord, int page = 1, int pageSize = 7,int subjectId = 0)
        {
            IPagedList<QuizTestViewModel> quiztests = null;
            var list = _db.QuizTests.Where(x=>x.status == TestStatus.Active && x.SubjectID == subjectId).ToList();

            if (!String.IsNullOrEmpty(keyWord))
            {
                list = list.Where(x => x.name.ToUpper().Contains(keyWord.ToUpper())).ToList();
            }

            quiztests = list.Select(x => new QuizTestViewModel()
            {
                TestID = x.TestID,
                name = x.name,
                TotalTime = (TimeQuiz)x.TotalTime,
                TotalMark = x.TotalMark,

            }).OrderByDescending(x => x.TestID).ToPagedList(page, pageSize);

            ViewBag.keyword = keyWord;
            ViewBag.subjectId = subjectId;
            ViewBag.Count = list.Count(); ;

            return View(quiztests);
        }
    }
}