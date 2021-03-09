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
    public class SubjectController : Controller
    {

        QuizContext _db = new QuizContext();

        [Authorize(Roles = "admin,teacher")]
        public ActionResult Index(string keyword, int page = 1, int pageSize = 7)
        {
            IPagedList<SubjectViewModel> subjects = null;
            var list = _db.Subjects.ToList();

            if (!String.IsNullOrEmpty(keyword))
            {
                list = list.Where(x => x.name.ToUpper().Contains(keyword.ToUpper())).ToList();
            }

            subjects = list.Select(x => new SubjectViewModel()
            {
                Id = x.ID,
                Name = x.name
            }).OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
            ViewBag.SearchString = keyword;

            ViewBag.Count = list.Count(); ;

            return View(subjects);
        }
        [HttpGet]
        [Authorize(Roles = "admin,teacher")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(SubjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = new Subject();
                model.name = viewModel.Name;
                _db.Subjects.Add(model);
                int result = _db.SaveChanges();
                if (result > 0)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "Thêm môn học mới thành công!");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm môn học mới thất bại!");
                }
            }
            return View("Create");
        }
        [HttpGet]
        [Authorize(Roles = "admin,teacher")]
        public ActionResult Update(int id)
        {
            var model = _db.Subjects.Find(id);

            var viewModel = new SubjectViewModel()
            {
                Id = model.ID,
                Name = model.name
            };

            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "admin,teacher")]
        public ActionResult Update(SubjectViewModel viewModel)
        {
            var model = _db.Subjects.Find(viewModel.Id);

            model.name = viewModel.Name;

            int result = _db.SaveChanges(); 
            if(result > 0)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "Cập nhật thành công!");
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật thất bại!");
            }
            return View("Update");
        }

        [Authorize(Roles = "admin,teacher")]
        public JsonResult Delete(int id)
        {
            try
            {
                var model = _db.Subjects.Find(id);
                _db.Subjects.Remove(model);
                _db.SaveChanges();
                return Json(model.name, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}