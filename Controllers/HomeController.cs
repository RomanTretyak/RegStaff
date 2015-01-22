using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegStaff.Models;
using System.Data.Entity;
using System.Reflection;
using System.Text;
using Common;
using System.Diagnostics;

namespace RegStaff.Controllers
{
    public class HomeController : Controller
    {
        //protected int PageSize { get { return 25; } }
        private string tmpstr = "0";

        PersonsContext db = new PersonsContext();
        RegStaff.Provider.CustomRoleProvider CustomRoleProvider = new Provider.CustomRoleProvider();
        delegate string DelegateDate(string s);

        Dictionary<int, string> OperationType = new Dictionary<int, string> { 
            {1, "Создание"},
            {2, "Изменение"},
            {3, "Удаление"}
        };

        Dictionary<int, string> RoleType = new Dictionary<int, string>{
            {1, "Admin"},
            {2, "HR"},
            {3, "SV"},
            {4, "Trainer"}
        };
        public ActionResult Index()
        {
            //IEnumerable<Person> persons = db.Person;
            //ViewBag.Persons = persons;
            return View();
        }
        public string ViewRole()
        {
            return "Ваша роль: Администратор";
        }
        [Authorize]
        public JsonResult PersonData(int? page)
        {
            try
            {
                IEnumerable<Person> persons = db.Person;
                var data = persons.Take(persons.Count()).ToList();

                string role = CustomRoleProvider.GetOneRoleForUser(User.Identity.Name);

                //пишем в лог
                cLogger.getLogger().logError(GetPropertyName() + User.Identity.Name
                    + ", Role: " + role + " , Data_count: " + data.Count());

                List<Person> list = new List<Models.Person> { };
                switch (role)
                {
                    case "Admin":
                        list = data;
                        break;
                    case "HR":
                        //нет требований к данным, после изменить
                        list = data;
                        break;
                    case "SV":
                        //нет требований к данным, после изменить
                        list = ListRolePerson(data);
                        break;
                    case "Trainer":
                        list = ListRolePerson(data);
                        break;
                    default:
                        cLogger.getLogger().logError(GetPropertyName() + default(JsonResult));
                        return default(JsonResult);
                }

                var result = new JsonResult
                {
                    Data = new { page, total = 1, records = list.Count(), rows = list }
                };
                cLogger.getLogger().logError(GetPropertyName() + list.Count());
                return result;
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex);
                return default(JsonResult);
            }
        }
        private List<Person> ListRolePerson(List<Person> CurrentPersons)
        {
            try
            {
                List<Person> result = new List<Models.Person> { };
                foreach (Person p in CurrentPersons)
                {
                    result.Add(new Person
                    {
                        Adjustment = p.Adjustment,
                        Category = p.Category,
                        CategoryType = p.CategoryType,
                        Comments = p.Comments,
                        DiffEducation = p.DiffEducation,
                        EndEducation = p.EndEducation,
                        FIO = p.FIO,
                        Id = p.Id,
                        IPN = p.IPN,
                        Login = p.Login,
                        OfficeLocation = p.OfficeLocation,
                        Project = p.Project,
                        Reason = p.Reason,
                        Result = p.Result,
                        SalaryNotOfficiall = tmpstr,
                        SalaryOfficaill = tmpstr,
                        SalaryType = p.SalaryType,
                        StartEducation = p.StartEducation,
                        StartWork = p.StartWork,
                        Status = p.Status,
                        SV = p.SV,
                        Trainer = p.Trainer,
                        WorkExperience = p.WorkExperience
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex.ToString());
                return default(List<Person>);
            }
        }

        [HttpPost]
        public void Edit(Person person)
        {
            try
            {
                FillWhoChange(OperationType[2], person);

                db.Person.Attach(person);
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex.ToString());
            }
        }
        private string DateTryParse(string inDate)
        {
            try
            {
                DateTime StartWork;
                string result;
                result = (DateTime.TryParse(inDate, out StartWork)) ?
                    StartWork.ToString("dd-MM-yyyy") :
                    default(DateTime).ToString();
                return result;
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex.ToString());
                return default(string);
            }
        }
        private string GetPropertyName()
        {  
            return new StackFrame(1).GetMethod().Name.ToString() + ": ";
        }

        [HttpPost]
        public void Create(Person person)
        {
            try
            {
                person = PersonAllDate(person);
                cLogger.getLogger().logError(GetPropertyName() + person.ToString());
                FillWhoChange(OperationType[1], person);
                db.Person.Add(person);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex);
            }
        }

        private Person PersonAllDate(Person person)
        {
            try
            {
                person.StartWork = DateTryParse(person.StartWork);
                person.StartEducation = DateTryParse(person.StartEducation);
                person.EndEducation = DateTryParse(person.EndEducation);
                return person;
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex.ToString());
                return default(Person);
            }
        }

        private void FillWhoChange(string OperationType, Person person)
        {
            try
            {
                RegStaff.Models.WhoChange NewItem = new WhoChange
                {
                    //Id = 1,
                    Login = LoginName(),
                    Date = Convert.ToString(DateTime.Now),
                    OType = OperationType,
                    FIO = GetUserFIO(person),
                    Get = OldPerson(person),
                    Set = OldPerson(person, true)
                };
                cLogger.getLogger().logError(GetPropertyName() + NewItem.ToString());
                db.WhoChange.Add(NewItem);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex);
            }
        }
        private string GetUserFIO(Person person)
        {
            try
            {
                if (string.IsNullOrEmpty(person.FIO))
                {
                    string result = default(string);

                    using (var context = new PersonsContext())
                    {
                        var SqlFio = context.Database.SqlQuery<string>(
                               "Select FIO from dbo.People where Id =" + person.Id).ToList();
                        result = SqlFio.First();
                    }
                    return result;
                }
                return person.FIO;
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex);
                return default(string);
            }
        }
        private string OldPerson(Person person, Boolean isNew = false)
        {
            try
            {
                using (var context = new PersonsContext())
                {
                    var blogNames = context.Database.SqlQuery<Person>(
                       "SELECT * FROM dbo.People").ToList();
                    string tmpStr = string.Empty;
                    foreach (Person p in blogNames)
                    {
                        if (p.Id == person.Id)
                        {
                            if (isNew)
                            {
                                tmpStr = BuildModelToString(p, person);
                            }
                            else
                            {
                                tmpStr = BuildModelToString(person, p);
                            }
                            break;
                        }
                    }
                    return tmpStr;
                }
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex.ToString());
                return default(string);
            }
        }

        private static string BuildModelToString(Person newPerson, Person bakPerson)
        {
            const char mySep = '|';
            string result = string.Empty;
            try
            {
                string[] arrNewPerson = DiffStr(newPerson).Split(mySep);
                string[] arrBewPerson = DiffStr(bakPerson).Split(mySep);

                for (int i = 0; i < arrNewPerson.Length; i++)
                {
                    if (arrNewPerson[i].ToString() != arrBewPerson[i].ToString())
                    //если есть изменения 
                    {
                        if (string.IsNullOrEmpty(arrNewPerson[i].ToString()) == false)
                            result = result + arrNewPerson[i].ToString() + ", ";
                    }
                }
                // IEnumerable<Person> p1 = new[] { newPerson };
                return result = (string.IsNullOrEmpty(result)) ?
                     default(string) : result.Substring(0, result.Length - 2);
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError("BuildModelToString: " + ex.ToString());
                return default(string);

            }
        }

        private static string DiffStr(Person p)
        {
            try
            {
                const char mySep = '|';
                string tmpStr = string.Empty;

                tmpStr = p.Adjustment + mySep
                    + p.Category + mySep
                    + p.CategoryType + mySep
                    + p.Comments + mySep
                    + p.DiffEducation + mySep
                    + p.EndEducation + mySep
                    + p.FIO + mySep
                    + p.IPN + mySep
                    + p.Login + mySep
                    + p.OfficeLocation + mySep
                    + p.Project + mySep
                    + p.Reason + mySep
                    + p.Result + mySep
                    + p.SalaryNotOfficiall + mySep
                    + p.SalaryOfficaill + mySep
                    + p.SalaryType + mySep
                    + p.StartEducation + mySep
                    + p.StartWork + mySep
                    + p.Status + mySep
                    + p.SV + mySep
                    + p.Trainer + mySep
                    + p.WorkExperience + mySep;
                return tmpStr;
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError("DiffStr: " + ex.ToString());
                return default(string);
            }
        }

        private string LoginName()
        {
            try
            {

                if (Request.IsAuthenticated)
                {
                    return User.Identity.Name;
                }
                else
                    return "Unknown login";
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex.ToString());
                return default(string);
            }
        }

        [HttpPost]
        public void Delete(Person person)
        {
            try
            {
                FillWhoChange(OperationType[3], person);
                db.Set<Person>().Attach(person);
                db.Entry(person).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                cLogger.getLogger().logError(GetPropertyName() + ex.ToString());
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
