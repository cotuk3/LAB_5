using EntityContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace EntityService.Tests;

[TestClass()]
public class InteractWithPersonTests
{
    public InteractWithPerson iwp = new InteractWithPerson();

    public static Student student =
        new Student("Bog", "Dan", "Male", true, "3-301", "BA 12345678", "2");

    #region SetFilePathTests
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    [TestMethod()]
    public void SetFilePath_4Extensions_Success(string filePath)
    {
        // arrange
        bool expected = true;
        string expectedS = null;

        //act
        var actual = iwp.SetFilePath(filePath);

        //assert
        bool res = actual.Item1 = expected && actual.Item2 == expectedS;

        Assert.IsTrue(res);
    }


    [TestMethod()]
    public void SetFilePath_JPG_Fail()
    {
        // arrange
        bool expected = false;
        string expectedS = "Unknow file extension!";

        //act
        var actual = iwp.SetFilePath("file.jpg");

        //assert
        bool res = actual.Item1 = expected && actual.Item2 == expectedS;

        Assert.IsFalse(res);
    }
    #endregion

    #region Add Student
    [TestMethod()]
    [DataRow("file1.dat")]
    [DataRow("file1.xml")]
    [DataRow("file1.json")]
    [DataRow("file1.txt")]
    public void Add_Student_Success(string filePath)
    {
        //arrange 
        List<Student> expected = new List<Student> { student };

        //act
        iwp.FilePath = filePath;
        iwp.Clear();
        iwp.Add(student);
        List<Student> actual = iwp.GetAll();
        Debug.WriteLine(actual[0]);
        //assert
        CollectionAssert.AreEquivalent(expected, actual);
    }

    [TestMethod()]
    [DataRow("file1.dat")]
    [DataRow("file1.xml")]
    [DataRow("file1.json")]
    [DataRow("file1.txt")]
    public void Add_2Students_Success(string filePath)
    {
        //arrange 
        List<Student> expected = new List<Student> { student, student };

        //act
        iwp.FilePath = filePath;
        iwp.Clear();
        iwp.Add(student);
        iwp.Add(student);
        List<Student> actual = iwp.GetAll();

        //assert
        CollectionAssert.AreEquivalent(expected, actual);
    }

    #endregion

    #region Add List<Student>
    [TestMethod()]
    [DataRow("file1.dat")]
    [DataRow("file1.xml")]
    [DataRow("file1.json")]
    [DataRow("file1.txt")]
    public void Add_List_Success(string filePath)
    {
        //arrange
        var expected = iwp.DefList;

        //act
        iwp.FilePath = filePath;
        iwp.Clear();
        iwp.Add(iwp.DefList);
        var actual = iwp.GetAll();

        //assert
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [DataRow("file1.dat")]
    [DataRow("file1.xml")]
    [DataRow("file1.json")]
    [DataRow("file1.txt")]
    public void Add_StudentThenList_Success(string filePath)
    {
        //arrange
        List<Student> expected = new List<Student>();
        expected.Add(student);
        expected.AddRange(iwp.DefList);


        //act
        iwp.FilePath = filePath;
        iwp.Clear();
        iwp.Add(student);
        iwp.Add(iwp.DefList);
        var actual = iwp.GetAll();

        //assert
        CollectionAssert.AreEquivalent(expected, actual);
    }

    #endregion

    #region Delete

    [TestMethod()]
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    public void Delete_Student_1_True(string filePath)
    {
        //arrange 
        int expected = 0;

        //act
        iwp.FilePath = filePath;
        iwp.Clear();
        iwp.Add(student);
        bool res = iwp.Delete(0);
        int actual = iwp.GetAll().Count;

        //assert
        Assert.IsTrue(res && actual == expected);
    }

    [TestMethod()]
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    public void Delete_Student_4_True(string filePath)
    {
        //arrange 
        var expected  = iwp.DefList;
        expected.RemoveAt(0);

        //act
        iwp.FilePath = filePath;
        iwp.Clear();
        iwp.Add(iwp.DefList);
        iwp.Delete(0);
        var actual = iwp.GetAll();

        //assert
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    public void Delete_Student_EmptyList_False(string filePath)
    {
        //arrange
        bool expected = false;

        //act
        iwp.FilePath = filePath;
        iwp.Clear();
        bool actual = iwp.Delete(0);

        //assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    public void Delete_Student_GreaterThanCount_False(string filePath)
    {
        //arrange
        bool expected = false;

        //act
        iwp.FilePath = filePath;
        iwp.Add(student);
        iwp.Clear();
        bool actual = iwp.Delete(1);

        //assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    public void Delete_Student_SmallerThanCount_False(string filePath)
    {
        //arrange
        bool expected = false;

        //act
        iwp.FilePath = filePath;
        iwp.Add(student);
        iwp.Clear();
        bool actual = iwp.Delete(-1);

        //assert
        Assert.AreEqual(expected, actual);
    }

    #endregion

    #region Clear
    [TestMethod()]
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    public void Clear_Success(string filePath)
    {
        //arrange
        string expected = "";
        //File.Create(filePath).Close();

        //act
        iwp.FilePath = filePath;
        bool res = iwp.Clear();
        string actual = File.ReadAllText(filePath);

        Debug.WriteLine(res);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void Clear_NotExistingFile_Fail()
    {
        //arrange
        bool expected = false;

        //act
        iwp.FilePath = "file21.txt";
        bool actual = iwp.Clear();


        Assert.AreEqual(expected, actual);
    }

    #endregion

    #region GetAll
    [TestMethod()]
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    public void GetAll_Success(string filePath)
    {
        //arrange
        var expected = iwp.DefList;

        //act
        iwp.FilePath = filePath;
        iwp.Add(iwp.DefList);
        var actual = iwp.GetAll();

        //assert
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [DataRow("file12.dat")]
    [DataRow("file12.xml")]
    [DataRow("file12.json")]
    [DataRow("file12.txt")]
    public void GetAll_NotExistingFile_Null(string filePath)
    {
        //arrange


        //act
        iwp.FilePath = filePath;
        var actual = iwp.GetAll();

        //assert
        Assert.IsNull(actual);
    }

    [TestMethod()]
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    public void GetAll_EmptyFile_Null(string filePath)
    {
        //arrange


        //act
        iwp.FilePath = filePath;
        iwp.Clear();
        var actual = iwp.GetAll();

        //assert
        Assert.IsNull(actual);
    }
    #endregion

    #region Search
    [TestMethod()]
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    public void Search3CourseInDorm_ChangingRooms_Success(string filePath)
    {
        //arrange
        List<Student> expected = new List<Student>()
        {
            new Student("Bohdan", "Liashenko", "Male", true, "3-101", "BL 12345678", "3"),
            new Student("Name", "Surname", "Female", true, "2-222", "NS 12345678", "3"),
            new Student("Someone", "Somebody", "Helicopter", true, "11-311", "NS 88888888", "3"),
        };

        //act
        iwp.FilePath = filePath;
        iwp.Clear();
        iwp.Add(iwp.DefList);
        var actual = iwp.Search3CourseInDorm();

        //assert
        CollectionAssert.AreEquivalent(expected, actual);

    }

    [TestMethod()]
    [DataRow("file.dat")]
    [DataRow("file.xml")]
    [DataRow("file.json")]
    [DataRow("file.txt")]
    public void Search3CourseInDorm_WritingToTheSameFileWithNewRooms_Success(string filePath)
    {
        //arrange
        List<Student> expected = new List<Student>()
        {
            new Student("Bohdan", "Liashenko", "Male", true, "3-101", "BL 12345678", "3"),
            new Student("Name", "Surname", "Female", true, "2-222", "NS 12345678", "3"),
            new Student("Someone", "Somebody", "Helicopter", true, "11-311", "NS 88888888", "3"),
            new Student("Somebody", "Someone", "Male", true, "9-601", "SB 77777777", "6"),
        };

        //act
        iwp.FilePath = filePath;
        iwp.Clear();
        iwp.Add(iwp.DefList);
        iwp.Search3CourseInDorm();
        var actual = iwp.GetAll();

        //assert
        CollectionAssert.AreEquivalent(expected, actual);

    }


    #endregion

    #region Create Student
    [TestMethod()]
    public void CreateStudent_DormFalse_Success()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", course = "2", residence = "Odesa", sID = "BB 12345678";
        bool dorm = false;
        Student expected = new Student(fName, lName, sex, dorm, residence, sID, course);

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);

        //assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void CreateStudent_DormTrue_Success()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", residence = "3-301", sID = "BB 12345678", course = "2";
        bool dorm = true;
        Student expected = new Student(fName, lName, sex, dorm,residence ,sID, course);

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);

        //assert
        Assert.AreEqual(expected, actual);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_WrongName_Fail()
    {
        //arrange
        string fName = "1Bohdan", lName = "Liashenko", sex = "Male", course = "2", residence = "Odesa", sID = "BB 12345678";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_WrongLName_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "1Liashenko", sex = "Male", course = "2", residence = "Odesa", sID = "BB 12345678";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_WrongSex_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "1Male", course = "2", residence = "Odesa", sID = "BB 12345678";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_WrongCourseLetter_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", course = "a", residence = "Odesa", sID = "BB 12345678";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_GreaterCourse_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", course = "7", residence = "Odesa", sID = "BB 12345678";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_SmallerCourse_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", course = "0", residence = "Odesa", sID = "BB 12345678";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_WrongResidenceDormFalse_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", course = "2", residence = "1Odesa", sID = "BB 12345678";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_WrongResidenceDormTrue_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", course = "2", residence = "Odesa", sID = "BB 12345678";
        bool dorm = true;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_WrongSIDLowerCaseLetters_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", course = "2", residence = "1Odesa", sID = "aa 12345678";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_WrongSIDWithoutSpace_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", course = "2", residence = "1Odesa", sID = "BB12345678";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_WrongSID9Digits_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", course = "2", residence = "Odesa", sID = "BB 123456789";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateStudent_WrongSID7Digits_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", course = "2", residence = "Odesa", sID = "BB 1234567";
        bool dorm = false;

        //act
        Student actual = iwp.CreateStudent(fName, lName, sex, dorm, residence, sID, course);
    }

    #endregion

    #region Create Gardener

    [TestMethod()]
    public void CreateGardener_Success()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", residence = "Odesa", employer = "Someone";

        Gardener expected = new Gardener(fName, lName, sex, residence, employer);

        //act
        Gardener actual = iwp.CreateGardener(fName, lName, sex, residence, employer);

        //assert
        Assert.AreEqual(expected, actual);
    }

    // starts with lower case letter
    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateGardener_WrongName_Fail()
    {
        //arrange
        string fName = "bohdan", lName = "Liashenko", sex = "Male", residence = "Odesa", employer = "Someone";

        //act
        Gardener actual = iwp.CreateGardener(fName, lName, sex, residence, employer);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateGardener_WrongLName_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "liashenko", sex = "Male", residence = "Odesa", employer = "Someone";

        //act
        Gardener actual = iwp.CreateGardener(fName, lName, sex, residence, employer);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateGardener_WrongSex_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "male", residence = "Odesa", employer = "Someone";

        //act
        Gardener actual = iwp.CreateGardener(fName, lName, sex, residence, employer);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateGardener_WrongResidence_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", residence = "odesa", employer = "Someone";

        //act
        Gardener actual = iwp.CreateGardener(fName, lName, sex, residence, employer);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateGardener_WrongEmployer_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", residence = "odesa", employer = "someone";

        //act
        Gardener actual = iwp.CreateGardener(fName, lName, sex, residence, employer);
    }

    #endregion

    #region Create Seller 

    [TestMethod()]
    public void CreateSeller_Success()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", residence = "Odesa", product = "Apples";

        Seller expected = new Seller(fName, lName, sex, residence, product);

        //act
        Seller actual = iwp.CreateSeller(fName, lName, sex, residence, product);

        //assert
        Assert.AreEqual(expected, actual);
    }

    //special characters in fields
    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateSeller_WrongName_Fail()
    {
        //arrange
        string fName = "Bohdan@", lName = "Liashenko", sex = "Male", residence = "Odesa", product = "Apples";

        //act
        Seller actual = iwp.CreateSeller(fName, lName, sex, residence, product);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateSeller_WrongLName_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "L@iashenko", sex = "Male", residence = "Odesa", product = "Apples";

        //act
        Seller actual = iwp.CreateSeller(fName, lName, sex, residence, product);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateSeller_WrongSex_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "M@ale", residence = "Odesa", product = "Apples";

        //act
        Seller actual = iwp.CreateSeller(fName, lName, sex, residence, product);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateSeller_WrongResidence_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", residence = "Ode@sa", product = "Apples";

        //act
        Seller actual = iwp.CreateSeller(fName, lName, sex, residence, product);
    }

    [ExpectedException(typeof(MyException))]
    [TestMethod()]
    public void CreateSeller_WrongProduct_Fail()
    {
        //arrange
        string fName = "Bohdan", lName = "Liashenko", sex = "Male", residence = "Odesa", product = "apples";

        //act
        Seller actual = iwp.CreateSeller(fName, lName, sex, residence, product);
    }

    #endregion
}

