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

        [Authorize(Roles = "admin,teacher")]
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
    }
}