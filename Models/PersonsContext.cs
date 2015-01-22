using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RegStaff.Models
{
    public class PersonsContext :DbContext
    {
        public PersonsContext ()
            : base("PersonsContext") 
        {
            
        }
        public DbSet<Person> Person { get; set; }
        public DbSet<WhoChange> WhoChange { get; set; }
    }
}