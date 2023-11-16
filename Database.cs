using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DDDProject
{
    internal class Database
    {
        string _fileName;

        public Database(string fileName)
        {
            _fileName = fileName;

            if (!File.Exists(fileName))
            {
                var connection = CreateConnection();
                CreateTables(connection);
            }
        }

        SQLiteConnection CreateConnection()
        {
            // Create a new database connection:
            SQLiteConnection connection = new SQLiteConnection($"Data Source = {_fileName}; Version = 3; New = True; Compress = True; ");

            // Open the connection:
            // try
            {
                connection.Open();
            }
            /*   catch (Exception ex)
               {

               } */
            return connection;
        }


        void CreateTables(SQLiteConnection conn)
        {
            string[] cmdStrings = { "CREATE TABLE Student (ID TEXT PRIMARY KEY, Name TEXT, Email TEXT)",
                                    "CREATE TABLE Staff (ID TEXT PRIMARY KEY, Name TEXT, Email TEXT)",
                                    "CREATE TABLE Course (ID TEXT PRIMARY KEY, Name TEXT)",
                                    "CREATE TABLE StudentCourse (StudentID TEXT, CourseID TEXT, PRIMARY KEY (StudentID, CourseID), FOREIGN KEY(StudentID) REFERENCES Student(ID), FOREIGN KEY(CourseID) REFERENCES Course(ID))",
                                    "CREATE TABLE StaffCourse (StaffID TEXT, CourseID TEXT, Role TEXT, PRIMARY KEY (StaffID, CourseID), FOREIGN KEY(StaffID) REFERENCES Staff(ID), FOREIGN KEY(CourseID) REFERENCES Course(ID))",
                                    "CREATE TABLE SupervisorStudent (StaffID TEXT, StudentID TEXT, PRIMARY KEY (StaffID, StudentID), FOREIGN KEY(StudentID) REFERENCES Student(ID), FOREIGN KEY(StaffID) REFERENCES Staff(ID))",
                                    "CREATE TABLE Meeting (ID INTEGER PRIMARY KEY AUTOINCREMENT, StudentID TEXT, StaffID TEXT, DateTime TEXT, FOREIGN KEY(StudentID) REFERENCES Student(ID), FOREIGN KEY(StaffID) REFERENCES Staff(ID))",
                                    "CREATE TABLE StudentEvaluation (ID INTEGER PRIMARY KEY AUTOINCREMENT, StudentID TEXT, DateTime TEXT, EvaluationRating INTEGER, ExtraNotes TEXT, FOREIGN KEY(StudentID) REFERENCES Student(ID))", };

            foreach (string cmdString in cmdStrings)
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmdString;
                cmd.ExecuteNonQuery();
            }
        }

        public void AddStudent(string id, string name, string email)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Student (ID, Name, Email) 
                                VALUES ($id, $name, $email)";

                // Bind the parameters to the query.
                cmd.Parameters.AddWithValue("$id", id);
                cmd.Parameters.AddWithValue("$name", name);
                cmd.Parameters.AddWithValue("$email", email);

                // Execute SQL.
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void AddCourse(string id, string name)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Course (ID, Name) 
                                VALUES ($id, $name)";

                // Bind the parameters to the query.
                cmd.Parameters.AddWithValue("$id", id);
                cmd.Parameters.AddWithValue("$name", name);

                // Execute SQL.
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void AddStaff(string id, string name, string email)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Staff (ID, Name, Email) 
                                VALUES ($id, $name, $email)";

                // Bind the parameters to the query.
                cmd.Parameters.AddWithValue("$id", id);
                cmd.Parameters.AddWithValue("$name", name);
                cmd.Parameters.AddWithValue("$email", email);

                // Execute SQL.
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void BookMeeting(string studentID, string staffID, string dateTime)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Meeting (StudentID, StaffID, DateTime) 
                                VALUES ($studentid, $staffid, $datetime)";

                // Bind the parameters to the query.
                cmd.Parameters.AddWithValue("$studentid", studentID);
                cmd.Parameters.AddWithValue("$staffid", staffID);
                cmd.Parameters.AddWithValue("$datetime", dateTime);

                // Execute SQL.
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void AddEvaluation(string studentID, string dateTime, int evaluationRating, string extraNotes)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO StudentEvaluation (StudentID, DateTime, EvaluationRating, ExtraNotes) 
                                VALUES ($studentid, $datetime, $evaluationrating, $extranotes)";

                // Bind the parameters to the query.
                cmd.Parameters.AddWithValue("$studentid", studentID);
                cmd.Parameters.AddWithValue("$datetime", dateTime);
                cmd.Parameters.AddWithValue("$evaluationrating", evaluationRating);
                cmd.Parameters.AddWithValue("$extranotes", extraNotes);

                // Execute SQL.
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void AddStudentToCourse(string studentID, string courseID)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO StudentCourse (StudentID, CourseID) 
                                VALUES ($studentid, $courseid)";

                // Bind the parameters to the query.
                cmd.Parameters.AddWithValue("$studentid", studentID);
                cmd.Parameters.AddWithValue("$courseid", courseID);

                // Execute SQL.
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void AddStaffToCourse(string staffID, string courseID)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO StaffCourse (StaffID, CourseID) 
                                VALUES ($staffid, $courseid)";

                // Bind the parameters to the query.
                cmd.Parameters.AddWithValue("$staffid", staffID);
                cmd.Parameters.AddWithValue("$courseid", courseID);

                // Execute SQL.
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void AssignSupervisorToStudent(string staffID, string studentID)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO SupervisorStudent (StaffID, StudentID) 
                                VALUES ($staffid, $studentid)";

                // Bind the parameters to the query.
                cmd.Parameters.AddWithValue("$staffid", staffID);
                cmd.Parameters.AddWithValue("$studentid", studentID);

                // Execute SQL.
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}