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
        enum Role
        {
            SeniorTutor,
            PersonalSupervisor,
            Student
        }

        Database db;

        string signedInID;
        Role role;

        static string CurrentDateStr()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }

        public void Run()
        {
            db = new Database("database.db");
            db.AddStaff("12345", "David", "david@hull.ac.uk");
            db.AddStudent("abcd1", "peter", "peter@hull.ac.uk", "12345");
            db.AddCourse("geo", "Geography", "12345");
            db.BookMeeting("abcd1", "12345", CurrentDateStr(), true);
            db.AddStudentToCourse("abcd1", "geo");
            db.AddStaffToCourse("12345", "geo");
            db.AddEvaluation("abcd1", CurrentDateStr(), 5, "was fine.");
            //_db.AddStudent("student2", "jack", "jack@hull.ac.uk", "24945433");

            string input;
            
            do
            {
                Console.WriteLine();
                if (signedInID == null)
                {
                    input = SignInMenu();
                }
                else
                {
                    input = MainMenu();
                }
            }
            while (input != "q");

        }

        private string SignInMenu()
        {
            string input;
            Console.WriteLine("1: Sign in as Student");
            Console.WriteLine("2: Sign in as Personal Supervisor");
            Console.WriteLine("3: Sign in as Senior Tutor");
            Console.WriteLine("q: Quit");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    {
                        SignIn(Role.Student);
                        break;
                    }
                case "2":
                    {
                        SignIn(Role.PersonalSupervisor);
                        break;
                    }
                case "3":
                    {
                        SignIn(Role.SeniorTutor);
                        break;
                    }
            }

            return input;
        }

        private string MainMenu()
        {
            string input;
            Console.WriteLine("1: Add self-evaluation");
            Console.WriteLine("q: Quit");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    {
                        AddSelfEvaluation();
                        break;
                    }
            }

            return input;
        }

        void SignIn(Role _role)
        {
            Console.WriteLine("\nEnter your ID");
            string id = Console.ReadLine();
            
            bool valid = true;
            if (_role == Role.Student)
            {
                valid = db.CheckStudentID(id);
            }
            else
            {
                valid = db.CheckStaffID(id);
            }
            if (valid)
            {
                if (_role == Role.Student || _role == Role.SeniorTutor)
                {
                    Console.WriteLine("Enter Course ID");
                    string courseID = Console.ReadLine();
                    if (_role == Role.Student)
                    {
                        valid = db.CheckStudentOnCourse(id, courseID);
                    }
                    else
                    {
                        valid = db.CheckStaffOnCourse(id, courseID);
                    }
                }
            }
            if (valid)
            {
                Console.WriteLine("Signed in successfully");
                signedInID = id;
                role = _role;
            }
            else
            {
                Console.WriteLine("Sign in failed");
            }
        }

        void AddSelfEvaluation()
        {
            int evaluation = -1;
            do
            {
                Console.WriteLine("How do you feel on a scale of 1-10 your course is going currently? ");
                string input = Console.ReadLine();
                int.TryParse(input, out evaluation);
            }
            while (evaluation < 1 || evaluation > 10);

            Console.WriteLine("Please give any extra notes you would like to add: ");
            string notes = Console.ReadLine();

            db.AddStudentEvaluation(signedInID,CurrentDateStr(), evaluation, notes);
        }
    }
}
