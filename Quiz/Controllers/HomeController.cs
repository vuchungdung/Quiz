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
    public class HomeController : Controller
    {
        QuizContext db = new QuizContext();
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                int UserID = (int)Session["UserID"];
                if (User.IsInRole("teacher")||User.IsInRole("admin"))
                {
                    ViewBag.countQuiz = db.Quizzes.Count(c => c.CreatorID == UserID);
                    ViewBag.countTest = db.QuizTests.Count(c => c.CreatorID == UserID);
                    ViewBag.countRoom = db.ActiveTests.Count(c => c.CreatorID == UserID);
                    return View("Dashboard");
                }
                return View();
            }
            return RedirectToAction("Login", "Authen");
        }
        public ActionResult TestResult(int? roomID, int page = 1, int pageSize = 7)
        {
            IPagedList<QuizResultViewModel> listResult = null;
            if (roomID == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                var list = db.QuizResults
                       .Where(c => c.ActiveTestID == roomID)
                       .Select(c => new QuizResultViewModel
                       {
                           StudentName = c.Student.fullname,
                           Name = c.ActiveTest.QuizTest.name,
                           SubmitDate = c.DoneAt,
                           Score = c.Score,
                           Status = c.Status,
                           MaxScore = c.ActiveTest.QuizTest.TotalMark,
                       }).ToList();
                listResult = list.OrderByDescending(x => x.Score).ToPagedList(page, pageSize);
                ViewBag.TestName = db.ActiveTests.Where(c => c.ID == roomID).First().QuizTest.name;
                ViewBag.Count = list.Count();
                return View(listResult);
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
    }
}