using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RegStaff.Models
{
    public class PersonsDbInitializer : DropCreateDatabaseIfModelChanges<PersonsContext>
    {
        protected override void Seed(PersonsContext db)
        {
           db.Person.Add(new Person{FIO = "Tretyak Roman Olegovich",Login = "RTretyak", Project ="Admin",
                                    Category = "Топ перформер", CategoryType ="ДОУ", SalaryOfficaill ="1000", SalaryNotOfficiall="3000",
                                    SalaryType = "Ставка + премия",OfficeLocation = "Киев", StartWork =  "2013-12-09 09:00:00",
                                    WorkExperience="1 год", SV="-", IPN="00000", Status="работает", Trainer="Нет",
                                    StartEducation = "2013-12-09 10:00:00",
                                    EndEducation = "2013-12-09 11:00:00",
                                    Adjustment = "-", DiffEducation ="1 день", Result = "Приступил", Comments="-"
           });
           base.Seed(db);
        }
    }
}