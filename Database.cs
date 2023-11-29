using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
            string[] cmdStrings = { "CREATE TABLE Student (ID TEXT PRIMARY KEY, Name TEXT, Email TEXT, SupervisorID TEXT, FOREIGN KEY(SupervisorID) REFERENCES Staff(ID))",
                                    "CREATE TABLE Staff (ID TEXT PRIMARY KEY, Name TEXT, Email TEXT)",
                                    "CREATE TABLE Course (ID TEXT PRIMARY KEY, Name TEXT, SeniorTutorID TEXT)",
                                    "CREATE TABLE StudentCourse (StudentID TEXT, CourseID TEXT, PRIMARY KEY (StudentID, CourseID), FOREIGN KEY(StudentID) REFERENCES Student(ID), FOREIGN KEY(CourseID) REFERENCES Course(ID))",
                                    "CREATE TABLE StaffCourse (StaffID TEXT, CourseID TEXT, Role TEXT, PRIMARY KEY (StaffID, CourseID), FOREIGN KEY(StaffID) REFERENCES Staff(ID), FOREIGN KEY(CourseID) REFERENCES Course(ID))",
                                    "CREATE TABLE Meeting (ID INTEGER PRIMARY KEY AUTOINCREMENT, StudentID TEXT, StaffID TEXT, DateTime TEXT, BookedByStudent INTEGER, FOREIGN KEY(StudentID) REFERENCES Student(ID), FOREIGN KEY(StaffID) REFERENCES Staff(ID))",
                                    "CREATE TABLE StudentEvaluation (ID INTEGER PRIMARY KEY AUTOINCREMENT, StudentID TEXT, DateTime TEXT, EvaluationRating INTEGER, ExtraNotes TEXT, FOREIGN KEY(StudentID) REFERENCES Student(ID))", };

            foreach (string cmdString in cmdStrings)
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmdString;
                cmd.ExecuteNonQuery();
            }
        }

        public void ExecuteNonQueryCommand(string sql, Dictionary<string, object> parameters)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                // Bind the parameters to the query.
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

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

        public List<NameValueCollection> ExecuteQueryCommand(string sql, Dictionary<string, object> parameters)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                // Bind the parameters to the query.
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                // Execute SQL.
                SQLiteDataReader reader = cmd.ExecuteReader();
                List<NameValueCollection> results = new();
                while (reader.Read())
                {
                    results.Add(reader.GetValues());
                }
                return results;
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
            return null;
        }

        public void AddStudent(string id, string name, string email, string supervisorID)
        {
            ExecuteNonQueryCommand(@"INSERT INTO Student (ID, Name, Email, SupervisorID) 
                                    VALUES ($id, $name, $email, $supervisorid)",
                                    new() { { "$id", id }, { "$name", name }, { "$email", email }, { "$supervisorid", supervisorID } });
        }

        public void AddCourse(string id, string name, string seniorTutorID)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Course (ID, Name, SeniorTutorID) 
                                VALUES ($id, $name, $seniortutorid)";

                // Bind the parameters to the query.
                cmd.Parameters.AddWithValue("$id", id);
                cmd.Parameters.AddWithValue("$name", name);
                cmd.Parameters.AddWithValue("$seniortutorid", seniorTutorID);

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

        public void BookMeeting(string studentID, string staffID, string dateTime, bool bookedByStudent)
        {
            SQLiteConnection conn = CreateConnection();
            try
            {
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Meeting (StudentID, StaffID, DateTime, BookedByStudent) 
                                VALUES ($studentid, $staffid, $datetime, $bookedbystudent)";

                // Bind the parameters to the query.
                cmd.Parameters.AddWithValue("$studentid", studentID);
                cmd.Parameters.AddWithValue("$staffid", staffID);
                cmd.Parameters.AddWithValue("$datetime", dateTime);
                cmd.Parameters.AddWithValue("$bookedbystudent", bookedByStudent);

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

        public bool CheckStaffID(string staffID)
        {
            var results = ExecuteQueryCommand("SELECT ID FROM Staff WHERE ID = $staffid",
                                new() { { "$staffid", staffID } });
            return results != null && results.Count > 0;
        }

        public bool CheckStudentID(string studentID)
        {
            var results = ExecuteQueryCommand("SELECT ID FROM Student WHERE ID = $studentid",
                                new() { { "$studentid", studentID } });
            return results != null && results.Count > 0;
        }

        public bool CheckStudentOnCourse(string studentID, string courseID)
        {
            var results = ExecuteQueryCommand("SELECT * FROM StudentCourse WHERE StudentID = $studentid AND CourseID = $courseid",
                               new() { { "$studentid", studentID }, { "$courseid", courseID } });
            return results != null && results.Count > 0;
        }

        public bool CheckStaffOnCourse(string staffID, string courseID)
        {
            var results = ExecuteQueryCommand("SELECT * FROM StaffCourse WHERE StaffID = $staffid AND CourseID = $courseid",
                               new() { { "$staffid", staffID }, { "$courseid", courseID } });
            return results != null && results.Count > 0;
        }

        public void AddStudentEvaluation(string studentID, string dateTime, int evaluation, string notes)
        {
            ExecuteNonQueryCommand(@"INSERT INTO StudentEvaluation (StudentID, DateTime, EvaluationRating, ExtraNotes) 
                                    VALUES ($studentid, $datetime, $rating, $notes)",
                                    new() { { "$studentid", studentID }, { "$rating", evaluation }, { "$notes", notes }, { "$datetime", dateTime } });
        }

        public NameValueCollection? GetStaffDetails(string id)
        {
            var results = ExecuteQueryCommand("SELECT * FROM Staff WHERE ID = $id",
                              new() { { "$id", id } });
            if (results == null || results.Count == 0)
            {
                return null;
            }
            else
            {
                return results.FirstOrDefault();
            }
        }

        public NameValueCollection? GetStudentDetails(string id)
        {
            var results = ExecuteQueryCommand("SELECT * FROM Student WHERE ID = $id",
                              new() { { "$id", id } });
            if (results == null || results.Count == 0)
            {
                return null;
            }
            else
            {
                return results.FirstOrDefault();
            }
        }

        public List<NameValueCollection> GetSupervisorStudents(string supervisorID)
        {
            return ExecuteQueryCommand("SELECT * FROM Student WHERE SupervisorID = $supervisorid",
                                        new() { { "$supervisorid", supervisorID } });
        }

        public List<NameValueCollection> GetStudentStatus(string supervisorID)
        {
            return ExecuteQueryCommand("SELECT * FROM Student, StudentEvaluation WHERE Student.SupervisorID = $supervisorid AND StudentEvaluation.StudentID = Student.ID",
                                        new() { { "$supervisorid", supervisorID } });
        }

        public List<NameValueCollection> GetAllStudentStatus()
        {
            return ExecuteQueryCommand("SELECT * FROM Student, StudentEvaluation WHERE StudentEvaluation.StudentID = Student.ID",
                                        new() { });
        }

        public List<NameValueCollection> GetAllMeetings()
        {
            return ExecuteQueryCommand("SELECT Student.Name AS StudentName, Staff.Name AS StaffName, DateTime FROM Meeting, Student, Staff WHERE Meeting.StudentID = Student.ID AND Meeting.StaffID = Staff.ID",
                                        new() { });
        }
    }

}