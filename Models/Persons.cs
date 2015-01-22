using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegStaff.Models
{
    public class Person
    {
        public Int32 Id { get; set; }
        public string FIO { get; set; }
        public string Login { get; set; }
        public string Project { get; set; }
        public string Category { get; set; }
        public string CategoryType { get; set; }
        public string SalaryOfficaill { get; set; }
        public string SalaryNotOfficiall { get; set; }
        public string SalaryType { get; set; }
        public string OfficeLocation { get; set; }
        public string StartWork { get; set; }
        public string WorkExperience { get; set; }
        public string SV { get; set; }
        public string IPN { get; set; }
        public string Status { get; set; }
        public string Trainer { get; set; }
        public string StartEducation { get; set; }
        public string EndEducation { get; set; }
        public string Adjustment { get; set; }
        public string DiffEducation { get; set; }
        public string Result { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }

    }
    public class WhoChange
    {
        public Int32 Id { get; set; }
        public string Login { get; set; }
        public string Date { get; set; }
        public string OType { get; set; }
        public string FIO { get; set; }
        public string Get { get; set; }
        public string Set { get; set; }
    }
    public class Project
    {
        public Int32 Id { get; set; }
        /*
        public DateTime StartEducation { get; set; }
        public DateTime EndEducation { get; set; }
         */
        public string Adjustment { get; set; }
        public string DiffEducation { get; set; }
        public string Result { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }
        
    }
}