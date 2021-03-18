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
    public class QuizController : Controller
    {

        QuizContext _db = new QuizContext();

        [Authorize(Roles = "admin")]
        public ActionResult Index(string keyWord, int page = 1, int pageSize = 7)
        {
            IPagedList<QuizViewModel> quizs = null;
            var list = _db.Quizzes.ToList();

            if (!String.IsNullOrEmpty(keyWord))
            {
                list = list.Where(x => x.name.ToUpper().Contains(keyWord.ToUpper())).ToList();
            }

            quizs = list.Select(x => new QuizViewModel()
            {
                Id = x.QuizID,
                Name = x.name,
                content = x.content,
                SubjectName = x.Subject.name,
                HardType = x.HardType,
                trueAnswer = x.trueAnswer

            }).OrderByDescending(x => x.Id).ToPagedList(page, pageSize);

            ViewBag.SearchString = keyWord;

            ViewBag.Count = list.Count(); ;

            return View(quizs);
        }

        [HttpGet]
        [Authorize(Roles ="admin,teacher")]
        public ActionResult Create()
        {
            var model = new QuizViewModel()
            {
                Subjects = Helper.getSubjectItem()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin,teacher")]
        public ActionResult Create(QuizViewModel viewModel)
        {
            var userID = _db.Users.Where(t => t.username == User.Identity.Name).First().ID;
            if (ModelState.IsValid)
            {
                var model = new Quizs();
                model.name = viewModel.Name;
                model.content = viewModel.content;
                model.answerA = viewModel.answerA;
                model.answerB = viewModel.answerB;
                model.answerC = viewModel.answerC;
                model.answerD = viewModel.answerD;
                model.CreatorID = userID;
                model.trueAnswer = viewModel.trueAnswer;
                model.SubjectID = viewModel.SubjectID;
                model.HardType = viewModel.HardType;
                model.CreateDate = DateTime.Now;
                _db.Quizzes.Add(model);
                var result = _db.SaveChanges();

                if (result > 0)
                {

                    ModelState.Clear();
                    ModelState.AddModelError("", "Bạn đã thêm câu hỏi thành công!");
                }
                else
                {
                    ModelState.AddModelError("", "Bạn đã thêm câu hỏi thất bại!");
                }
            }
            viewModel = new QuizViewModel();
            viewModel.Subjects = Helper.getSubjectItem();
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var model = _db.Quizzes.Find(id);

            var viewModel = new QuizViewModel()
            {
                Name = model.name,
                content = model.content,
                answerA = model.answerA,
                answerB = model.answerB,
                answerC = model.answerC,
                answerD = model.answerD,
                trueAnswer = model.trueAnswer,
                SubjectID = model.SubjectID,
                HardType = model.HardType
            };

            viewModel.Subjects = Helper.getSubjectItem();
            return View(viewModel);
        }
        [Authorize(Roles = "admin,teacher")]
        [HttpPost]
        public ActionResult Update(QuizViewModel viewModel)
        {
            var model = _db.Quizzes.Find(viewModel.Id);

            model.name = viewModel.Name;
            model.content = viewModel.content;
            model.answerA = viewModel.answerA;
            model.answerB = viewModel.answerB;
            model.answerC = viewModel.answerC;
            model.answerD = viewModel.answerD;
            model.trueAnswer = viewModel.trueAnswer;
            model.SubjectID = viewModel.SubjectID;
            model.HardType = viewModel.HardType;

            int result = _db.SaveChanges();

            if (result > 0)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "Cập nhật thành công!");
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật thất bại!");
            }

            viewModel = new QuizViewModel();
            viewModel.Subjects = Helper.getSubjectItem();
            return View(viewModel);
        }
        [Authorize(Roles = "admin,teacher")]
        public JsonResult Delete(int id)
        {
            try
            {
                var model = _db.Quizzes.Find(id);
                _db.Quizzes.Remove(model);
                _db.SaveChanges();
                return Json(model.name, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [Authorize(Roles = "admin,teacher")]
        public ActionResult MyIndex(string keyWord, int page = 1, int pageSize = 7)
        {
            var userID = _db.Users.Where(t => t.username == User.Identity.Name).First().ID;
            IPagedList<QuizViewModel> quizs = null;

            var list = _db.Quizzes.Where(x=>x.CreatorID == userID).ToList();

            if (!String.IsNullOrEmpty(keyWord))
            {
                list = list.Where(x => x.name.ToUpper().Contains(keyWord.ToUpper())).ToList();
            }

            quizs = list.Select(x => new QuizViewModel()
            {
                Id = x.QuizID,
                Name = x.name,
                content = x.content,
                SubjectName = x.Subject.name,
                HardType = x.HardType,
                trueAnswer = x.trueAnswer

            }).OrderByDescending(x => x.Id).ToPagedList(page, pageSize);

            ViewBag.keyWord = keyWord;

            ViewBag.Count = list.Count(); ;

            return View(quizs);
        }
    }
}