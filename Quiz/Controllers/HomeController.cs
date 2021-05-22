using Dapper;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using PagedList;
using Quiz.Models;
using Quiz.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                var listActiveTest = from a in db.ActiveTests
                                     join q in db.QuizTests
                                     on a.QuizTestID equals q.TestID
                                     join s in db.Subjects
                                     on q.SubjectID equals s.ID
                                     select new DropdownViewModel()
                                     {
                                         SubjectName = s.name,
                                         NameQuizTest = q.name,
                                         Code = a.Code
                                     };
                if (User.IsInRole("teacher")||User.IsInRole("admin"))
                {
                    ViewBag.countQuiz = db.Quizzes.Count(c => c.CreatorID == UserID);
                    ViewBag.countTest = db.QuizTests.Count(c => c.CreatorID == UserID);
                    ViewBag.countRoom = db.ActiveTests.Count(c => c.CreatorID == UserID);
                    return View("Dashboard", listActiveTest.ToList());
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
                ViewBag.roomId = roomID;
                return View(listResult);
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
        public ActionResult Export(int? roomID)
        {
            DataTable Dt = new DataTable();
            Dt.Columns.Add("Họ và tên", typeof(string));
            Dt.Columns.Add("Đề thi", typeof(string)); 
            Dt.Columns.Add("Ngày thi", typeof(string));
            Dt.Columns.Add("Điểm", typeof(string));
            Dt.Columns.Add("Trạng thái", typeof(string));

            var list = db.QuizResults
                       .Where(c => c.ActiveTestID == roomID)
                       .Select(c => new QuizResultViewModel
                       {
                           StudentName = c.Student.fullname,
                           Name = c.ActiveTest.QuizTest.name,
                           SubmitDate = c.DoneAt,
                           Point = c.Score.ToString() + "/" + c.ActiveTest.QuizTest.TotalMark.ToString(),
                           Status = c.Status,
                       }).ToList();
            foreach (var item in list)
            {
                DataRow row = Dt.NewRow();
                row[0] = item.StudentName;
                row[1] = item.Name;
                row[2] = item.SubmitDate;
                row[3] = item.Point;
                row[4] = item.Status;
                Dt.Rows.Add(row);

            }
            var memoryStream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excelPackage = new ExcelPackage(memoryStream))
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                worksheet.DefaultRowHeight = 18;


                worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.DefaultColWidth = 20;
                worksheet.Column(2).AutoFit();

                Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
            }
            byte[] data = Session["DownloadExcel_FileManager"] as byte[];
            return File(data, "application/octet-stream", "DiemThi.xlsx");
        }
        public JsonResult Statistical(string code)
        {
            var sql = @"select qr.Score, count(qr.Score) as Amount, q.TotalMark, q.name
                        from dbo.ActiveTests as a, dbo.QuizResults as qr, dbo.QuizTests as q 
                        where qr.ActiveTestID = a.ID and q.TestID = a.QuizTestID and a.Code = '" + code+"'"+
                        "group by qr.Score, q.TotalMark, q.name";
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-G6EPV8T\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True;"))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var result = conn.Query<ChartViewModel>(sql, null, null, true, 120, CommandType.Text);

                ResponseChartViewModel listResult = new ResponseChartViewModel();

                foreach(var item in result)
                {
                    listResult.name = item.name;
                    listResult.totalMark = item.TotalMark;
                    ChartView elm = new ChartView();
                    elm.Amount = item.Amount;
                    elm.Score = item.Score;
                    listResult.Charts.Add(elm);
                }

                return Json(listResult, JsonRequestBehavior.AllowGet);
            }
        }
    }
}