using Quiz.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SessionFilter());
        }
    }
    public class SessionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.Session["UserID"] == null)
            {
                QuizContext db = new QuizContext();
                bool Exist = db.Users.Any(e => e.username == HttpContext.Current.User.Identity.Name);
                if (Exist)
                {
                    var user = db.Users.Where(e => e.username == HttpContext.Current.User.Identity.Name).First();
                    HttpContext.Current.Session["UserID"] = user.ID;
                    HttpContext.Current.Session["Name"] = user.fullname;
                }
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
