using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityContext.Entities.Exception;
public class PersonException : System.Exception
{
	public PersonException()
		:base()
	{

	}

	public PersonException(string input)
		:base($"{input} was in wrong format!")
	{

	}
}
