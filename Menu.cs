using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Globalization;

namespace DDDProject
{
    internal class Menu
    {
        Database _db;

        public void Run()
        {
            _db = new Database("database.db");
            _db.AddStaff("12345", "David", "david@hull.ac.uk");
            _db.AddStudent("abcd1", "peter", "peter@hull.ac.uk", "12345");
            _db.AddCourse("geo", "Geography", "12345");
            _db.BookMeeting("abcd1", "12345", CurrentDateStr());
            _db.AddStudentToCourse("abcd1", "geo");
            _db.AddStaffToCourse("12345", "geo");
            _db.AddEvaluation("abcd1", CurrentDateStr(), 5, "was fine.");

            static string CurrentDateStr()
            {
                return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            }
        }
    }
}
