using RegStaff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegStaff.Controllers
{
    public class HistoryController : Controller
    {
        PersonsContext db = new PersonsContext();
        RegStaff.Provider.CustomRoleProvider CustomRoleProvider = new Provider.CustomRoleProvider();

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Data(int? page)
        {
            try
            {
                if (CustomRoleProvider.GetOneRoleForUser(User.Identity.Name) == "HR" ||
                CustomRoleProvider.GetOneRoleForUser(User.Identity.Name) == "Admin")
                {
                    IEnumerable<WhoChange> WhoChange = db.WhoChange;
                    var data = WhoChange.ToList();
                    string Role = CustomRoleProvider.GetOneRoleForUser(User.Identity.Name);
                    List<WhoChange> list = new List<Models.WhoChange> { };
                    list = data.ToList();

                    var result = new JsonResult
                    {
                        Data = new { page, total = 1, records = list.Count(), rows = list }
                    };
                    return result;
                }
                else
                    return default(System.Web.Mvc.JsonResult);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
  