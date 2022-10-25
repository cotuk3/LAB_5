using EntityContext.Entities.Interfaces;
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
public class Person : IVerticalSleeping
{
    #region Fields

    string _firstName;
    string _lastName;
    string _sex;
    string _residence;
    protected bool _isLivingInDorm;

    [NonSerialized]
    protected Regex validName = new Regex(@"^[A-Za-z ]+$");

    #endregion

    #region Ctors
    public Person()
    {

    }
    public Person(string firstName, string lastName, string sex, string residence, bool isLivingInDorm = false)
    {
        FirstName = firstName;
        LastName = lastName;
        Sex = sex;
        _isLivingInDorm = isLivingInDorm;
        Residence = residence;
    }
    #endregion

    #region Properties
    public bool IsLivingInDorm 
    {
        get => _isLivingInDorm;
        set => _isLivingInDorm = value;
    }
    public string FirstName
    {
        get => _firstName;
        
        set
        {
            if(validName.IsMatch(value))
                _firstName = value;
            else
                throw new PersonException(nameof(FirstName));
        }
    }
    public string LastName
    {
        get => _lastName;
        set
        {
            if(validName.IsMatch(value))
                _lastName = value;
            else
                throw new PersonException(nameof(LastName));
        }
    }
    public string Sex
    {
        get => _sex;
        set
        {
            if(validName.IsMatch(value))
                _sex = value;
            else
                throw new PersonException(nameof(Sex));

        }
    }
    public string Residence
    {
        get => _residence; 
        set
        {
            if(_isLivingInDorm && Regex.IsMatch(value, @"^\d{1,2}-\d{3}$"))
                _residence = value;
            else if(validName.IsMatch(value))
                _residence = value;
            else
                throw new PersonException(nameof(Residence));    
        }
    }

    #endregion

    public virtual string SleepVertical()
    {
        string res =$"{GetType().Name} is sleeping vertical in {_residence}";
        return res;
    }
}
