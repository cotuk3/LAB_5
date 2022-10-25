using EntityContext.Entities.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EntityContext;
[Serializable]
public class Student : Person
{
    #region Fields

    string _studentId;
    string _course;

    #endregion

    #region Ctors

    public Student()
    {

    }
    public Student(string firstName, string lastName, string sex, bool isLivingInDorm, string residence,
            string studentId, string course)
        :base(firstName, lastName, sex, residence, isLivingInDorm)
    {
        StudentId = studentId;  
        Course = course;
    }

    #endregion

    #region Properties

    public string StudentId
    {
        get =>_studentId;
        set
        {
            Regex validSID = new Regex(@"^[A-Z]{2}\s\d{8}$");
            if(validSID.IsMatch(value))
                _studentId = value;
            else
                throw new PersonException(nameof(StudentId));
        }
    }
    
    public string Course
    {
        get => _course; 
        set
        {
            Regex validCourse = new Regex("[1-6]");
            if(validCourse.IsMatch(value))
                _course = value;
            else
            throw new PersonException(nameof(Course));
    }
    }

    #endregion

    public string Study()
    {
        string res = $"{GetType().Name} with name: {FirstName} {LastName} is studying in {Course} year.";
        return res;
    }
    public override string ToString()
    {
        string res = $"Student {FirstName}{LastName}\n" +
            $"\"{{ \"firstname\": \"{FirstName}\",\n" +
            $"\"lastname\": \"{LastName}\",\n" +
            $"\"sex\": \"{Sex}\",\n" +
            $"\"residence\": \"{Residence}\",\n" +
            $"\"course\": \"{Course}\",\n" +
            $"\"studentId\": \"{StudentId}\"}};";

        return res;
    }

    public override bool Equals(object? obj)
    {
        Student student = obj as Student;
        if(obj is not null)
        {
            if(FirstName == student.FirstName && LastName == student.LastName &&
                Sex == student.Sex && Residence == student.Residence && 
                Course == student.Course && StudentId == student.StudentId)
                return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return FirstName.GetHashCode() * LastName.GetHashCode() * Sex.GetHashCode() 
            * Residence.GetHashCode() * Course.GetHashCode() * StudentId.GetHashCode();
    }
}
