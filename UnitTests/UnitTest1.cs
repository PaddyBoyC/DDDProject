using DDDProject;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        string testFileName = "test.db";

        [TestMethod]
        public void TestAddStudent()
        {
            if (File.Exists(testFileName))
            {
                File.Delete(testFileName);
            }
            Database db = new Database (testFileName);
            db.AddStudent("abcd1", "Brian", "brain-beck@hull.ac.uk", "12345");
            var result = db.GetStudentDetails("abcd1");
            Assert.IsNotNull(result);
            Assert.AreEqual(result["Name"], "Brian");
        }
    }
}