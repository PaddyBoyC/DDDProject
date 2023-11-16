using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace DDDProject
{
    internal class Menu
    {
        Database _db;

        public void Run()
        {
            _db = new Database("database.db");
            _db.AddStudent("abcd1", "peter", "peter@hull.ac.uk");
        }
    }
}
