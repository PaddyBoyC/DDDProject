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
        int signedInID = -1;
        bool isStudent = false;

        public void Run()
        {
            _db = new Database("database.db");
            _db.AddStaff("12345", "David", "david@hull.ac.uk");
            _db.AddStudent("abcd1", "peter", "peter@hull.ac.uk", "12345");
            _db.AddCourse("geo", "Geography", "12345");
            _db.BookMeeting("abcd1", "12345", CurrentDateStr(), true);
            _db.AddStudentToCourse("abcd1", "geo");
            _db.AddStaffToCourse("12345", "geo");
            _db.AddEvaluation("abcd1", CurrentDateStr(), 5, "was fine.");
            //_db.AddStudent("student2", "jack", "jack@hull.ac.uk", "24945433");

            static string CurrentDateStr()
            {
                return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            }

            string input;
            
            do
            {
                Console.WriteLine("1: Sign in as Staff");
                Console.WriteLine("2: Sign in as Student");
                Console.WriteLine("3: Exit");
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        {
                            SignInAsStaff();
                            break;
                        }
                }
            }
            while (input != "3");

        }

        public void SignInAsStaff()
        {
            Console.WriteLine("\nEnter Staff ID");
            string id = Console.ReadLine();
            bool valid = _db.CheckStaffID(id);
            Console.WriteLine(valid);
        }
    }
}
