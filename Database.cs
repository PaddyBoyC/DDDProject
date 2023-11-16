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
                                    "CREATE TABLE StudentCourse (StudentID TEXT, CourseID TEXT, PRIMARY KEY (StudentID, CourseID))",
                                    "CREATE TABLE StaffCourse (StaffID TEXT, CourseID TEXT, Role TEXT, PRIMARY KEY (StaffID, CourseID))",
                                    "CREATE TABLE SupervisorStudent (StaffID TEXT, StudentID TEXT, PRIMARY KEY (StaffID, CourseID))",
                                    "CREATE TABLE Meeting (ID INTEGER PRIMARY KEY AUTOINCREMENT, StudentID TEXT, StaffID TEXT, DateTime TEXT",
                                    "CREATE TABLE StudentEvaluation (ID INTEGER PRIMARY KEY AUTOINCREMENT, StudentID TEXT, DateTime TEXT, EvaluationRating INTEGER, ExtraNotes TEXT", };

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
                cmd.CommandText = @"INSERT INTO Student Evaluation (StudentID, DateTime, EvaluationRating, ExtraNotes) 
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


    }
}
