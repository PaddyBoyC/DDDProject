using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Globalization;
using System.Collections.Specialized;

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
        Role signedInRole;

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
                else if (signedInRole == Role.PersonalSupervisor)
                {
                    input = PersonalSupervisorMenu();
                }
                else if (signedInRole == Role.Student)
                {
                    input = StudentMenu();
                }
                else
                {
                    input = SeniorTutorMenu();
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

        private string StudentMenu()
        {
            string input;
            Console.WriteLine("1: Add self-evaluation");
            Console.WriteLine("2: Book meeting");
            Console.WriteLine("q: Quit");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    {
                        AddSelfEvaluation();
                        break;
                    }
                case "2":
                    {
                        BookMeeting();
                        break;
                    }
            }

            return input;
        }

        private string PersonalSupervisorMenu()
        {
            string input;
            Console.WriteLine("1: View students status");
            Console.WriteLine("2: Book meeting");
            Console.WriteLine("q: Quit");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    {
                        ViewStudentStatus();
                        break;
                    }
                case "2":
                    {
                        BookMeeting();
                        break;
                    }
            }

            return input;
        }

        private string SeniorTutorMenu()
        {
            string input;
            Console.WriteLine("1: View students status");
            Console.WriteLine("2: View Students/Personal Supervisor meetings");
            Console.WriteLine("q: Quit");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    {
                        ViewStudentStatus();
                        break;
                    }
                case "2":
                    {
                        ShowAllMeetings();
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
                signedInRole = _role;
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

        void BookMeeting()
        {
            string staffID, studentID;
            if (signedInRole == Role.Student)
            {
                studentID = signedInID;
                // find out the ID of the staff member the student wants to book with
                Console.WriteLine("Enter the ID of the staff member you want to book a meeting with");
                string input = Console.ReadLine();
                var staffDetails = db.GetStaffDetails(input);
                if (staffDetails == null)
                {
                    Console.WriteLine("Invalid staff ID");
                    return;
                }
                staffID = input;
            }
            else
            {
                staffID = signedInID;
                Console.WriteLine("Enter the ID of the student member you want to book a meeting with");
                string input = Console.ReadLine();
                var studentDetails = db.GetStudentDetails(input);
                if (studentDetails == null)
                {
                    Console.WriteLine("Invalid student ID");
                    return;
                }
                studentID = input;
            }

            Console.WriteLine("Enter the desired date and time for the meeting (e.g. 28/02/2024 13:00): ");
            DateTime? dateTime = InputDateTime();
            if (dateTime.HasValue)
            {
                db.BookMeeting(studentID, staffID, dateTime.Value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), signedInRole == Role.Student);
            }
            else
            {
                Console.WriteLine("Invalid date/time");
            }
        }

        static DateTime? InputDateTime()
        {
            Console.WriteLine("Enter a date: ");
            DateTime userDateTime;
            if (DateTime.TryParse(Console.ReadLine(), out userDateTime))
            {
                return userDateTime;
            }
            else
            {
                return null;
            }
        }

        void ViewStudentStatus()
        {
            List<NameValueCollection> results;
            if (signedInRole == Role.PersonalSupervisor)
            {
                results = db.GetStudentStatus(signedInID);
            }
            else
            {
                results = db.GetAllStudentStatus();
            }
            foreach (var result in results)
            {
                Console.WriteLine($"{result["Name"]} {result["Email"]} {result["EvaluationRating"]} {result["ExtraNotes"]} {result["DateTime"]}");
            }
        }

        void ShowAllMeetings()
        {
            var results = db.GetAllMeetings();
            foreach (var result in results)
            {
                Console.WriteLine($"{result["StudentName"]} {result["StaffName"]} {result["DateTime"]}");
            }
        }
    }
}
