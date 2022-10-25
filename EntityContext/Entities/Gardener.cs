using EntityContext.Entities.Interfaces;
using EntityContext.Entities.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace EntityContext;
[Serializable]
public class Gardener : Person, IWateringPlants
{
    string? _employer;
    public Gardener()
    {

    }

    public Gardener(string firstName, string lastName, string sex, string residence, string employer)
		:base(firstName, lastName, sex, residence)
	{
		Employer = employer;
	}


	public string? Employer
	{
        get => _employer;
        set
        {
            if(validName.IsMatch(value))
                _employer = value;
            else
                throw new PersonException(nameof(Employer));
        }
	}

    #region Methods
    public string WaterThePlants()
	{
		string res = $"{GetType().Name} with name: {FirstName} {LastName} is watering the plants in flowerbed.";
		return res;
	}

    public string CutTheLawn()
    {
        string res = $"{GetType().Name} with name: {FirstName} {LastName} is  cutting the lawn in {Employer} residence.";
        return res;
    }
    #endregion

    public override string ToString()
    {
        string res = $"Gardener {FirstName}{LastName}\n" +
            $"\"{{ \"firstname\": \"{FirstName}\",\n" +
            $"\"lastname\": \"{LastName}\",\n" +
            $"\"sex\": \"{Sex}\",\n" +
            $"\"residence\": \"{Residence}\",\n" +
            $"\"employer\": \"{Employer}\"}};";

        return res;
    }

    public override bool Equals(object? obj)
    {
        Gardener gardener = obj as Gardener;
        if(obj is not null)
        {
            if(FirstName == gardener.FirstName && LastName == gardener.LastName &&
                Sex == gardener.Sex && Residence == gardener.Residence &&
                   Employer == gardener.Employer)
                return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return FirstName.GetHashCode() * LastName.GetHashCode() * Sex.GetHashCode()
            * Residence.GetHashCode() * Employer.GetHashCode();
    }
}
