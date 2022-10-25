﻿using System.Xml.Serialization;

namespace EntityContext;

public class XMLProvider : DataProvider
{
    public XMLProvider(Type type)
        : base(type)
    {

    }

    public override void Serialize(object graph, string filePath)
    {
        using (FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            XmlSerializer xs = new XmlSerializer(_type, new Type[] { typeof(Student), typeof(Gardener), typeof(Seller) });
            xs.Serialize(fileStream, graph);
        }
    }

    public override object Deserialize(string filePath)
    {
        object obj;
        using (FileStream fileStream = File.OpenRead(filePath))
        {
            XmlSerializer xs = new XmlSerializer(_type, new Type[] { typeof(Student), typeof(Gardener), typeof(Seller) });
            try
            {
                obj = xs.Deserialize(fileStream);
                return obj;
            }
            catch
            {
                return null;
            }
        }

    }
}
