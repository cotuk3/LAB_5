using EntityContext.Entities.Exception;
using EntityContext.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityContext;
[Serializable]
public class Seller : Person, IWateringPlants
{
    string? _product;
    public Seller()
    {

    }

    public Seller(string firstName, string lastName, string sex, string residence, string product)
    : base(firstName, lastName, sex, residence)
    {
        Product = product;
    }

    public string? Product
    {
        get => _product;
        set
        {
            if(validName.IsMatch(value))
                _product = value;
            else
                throw new PersonException(nameof(Product));
        }
    }

    #region Methods
    public string WaterThePlants()
    {
        string res = $"{GetType().Name} with name: {FirstName} {LastName} is watering the plants in vase.";
        return res;
    }
    public string SellTheProduct()
    {
        string res = $"{GetType().Name} with name: {FirstName} {LastName} is selling {Product}.";
        return res;
    }
    #endregion

    #region Object
    public override string ToString()
    {
        string res = $"Seller {FirstName}{LastName}\n" +
            $"\"{{ \"firstname\": \"{FirstName}\",\n" +
            $"\"lastname\": \"{LastName}\",\n" +
            $"\"sex\": \"{Sex}\",\n" +
            $"\"residence\": \"{Residence}\",\n" +
            $"\"product\": \"{Product}\"}};";
        return res;
    }

    public override bool Equals(object? obj)
    {
        Seller seller = obj as Seller;
        if(obj is not null)
        {
            if(FirstName == seller.FirstName && LastName == seller.LastName &&
                Sex == seller.Sex && Residence == seller.Residence &&
                   Product == seller.Product)
                return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return FirstName.GetHashCode() * LastName.GetHashCode() * Sex.GetHashCode()
            * Residence.GetHashCode() * Product.GetHashCode();
    }
    #endregion

}
