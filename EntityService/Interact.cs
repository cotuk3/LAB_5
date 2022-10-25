using EntityContext;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace EntityService;

public class InteractWithPerson
{
	#region Fields

	string _filePath;
	string _extension;

	ArgumentException wrongFile = new ArgumentException("Unknow file extension!");

	static Dictionary<string, Func<string, object>> deser = new Dictionary<string, Func<string, object>>()
	{
		{ ".dat", (filePath) => new BinaryProvider(typeof(List<Student>)).Deserialize(filePath) },
		{ ".xml", (filePath) => new XMLProvider(typeof(List<Student>)).Deserialize(filePath) },
		{ ".json", (filePath) => new JSONProvider(typeof(List<Student>)).Deserialize(filePath) },
		{ ".txt", (filePath) => new CustomProvider(typeof(List<>), typeof(Student)).Deserialize(filePath) }
	};

	static Dictionary<string, Action<object, string>> ser = new Dictionary<string, Action<object, string>>()
	{
		{".dat", (graph, filePath) => new BinaryProvider(typeof(List<Student>)).Serialize(graph, filePath) },
		{".xml", (graph, filePath) => new XMLProvider(typeof(List<Student>)).Serialize(graph, filePath) },
		{".json", (graph, filePath) => new JSONProvider(typeof(List<Student>)).Serialize(graph, filePath)},
		{".txt", (graph, filePath) => new CustomProvider(typeof(List<>), typeof(Student)).Serialize(graph, filePath)}
	};
	#endregion

	#region ctorS
	public InteractWithPerson(string filePath)
	{
		FilePath = filePath;
	}
	public InteractWithPerson()
	{
	}
	#endregion

	#region Properties
	public string FilePath
	{
		get => _filePath;
		set
		{
			string extension = Path.GetExtension(value);
			if(extension == ".dat" || extension == ".xml" || extension == ".json" || extension == ".txt")
			{
				_filePath = value;
				_extension = Path.GetExtension(_filePath);
			}

			else
				throw wrongFile;
		}
	}
	public (bool, string) SetFilePath(string filePath)
	{
		try
		{
			FilePath = filePath;
			return (true, null);
		}
		catch
		{
			return (false, wrongFile.Message);
		}
	}

	public List<Student> DefList
	{
		get
		{
			return new List<Student>()
			{
				new Student("Bohdan", "Liashenko", "Male", true, "3-301", "BL 12345678", "3"),
				new Student("Name", "Surname", "Female", true, "2-322", "NS 12345678", "3"),
				new Student("Someone", "Somebody", "Helicopter", true, "11-111", "NS 88888888", "3"),
				new Student("Somebody", "Someone", "Male", true, "9-601", "SB 77777777", "6"),
			};
		}
	}
	#endregion

	#region Write to File
	public void Add(Student student)
	{
		List<Student> res;
		if(File.Exists(_filePath))
		{
			try
			{
				res = deser[_extension](_filePath) as List<Student>;
			}
			catch
			{
				res = new List<Student>();
			}

			if(res == null)
			{
				res = new List<Student>();
			}

		}
		else
		{
			res = new List<Student>();
		}
		res.Add(student);
		ser[_extension](res, _filePath);
	}
	public void Add(List<Student> list)
	{
		if(File.Exists(_filePath))
		{
			List<Student> res;
			try
			{
				res = deser[_extension](_filePath) as List<Student>;
			}
			catch
			{
				res = new List<Student>();
			}
			if(res == null)
				res = new List<Student>();

			foreach(var student in res)
				list.Add(student);
		}
		ser[_extension](list, _filePath);
	}
	public bool Delete(int index)
	{
		if(!File.Exists(_filePath))
			return false;

		List<Student> list;

		try
		{
			list = GetAll();
		}
		catch(SerializationException)
		{
			list = null;
		}

		if(list == null || index >= list.Count || index < 0)
			return false;

		list.RemoveAt(index);
		File.Delete(_filePath);
		Add(list);
		return true;
	}
	public void Clear()
	{
		DataProvider.ClearFile(_filePath);
	}
	#endregion

	#region Read From File
	public List<Student> GetAll()
	{
		if(File.Exists(_filePath))
		{


			try
			{
				return deser[_extension](_filePath) as List<Student>;
			}
			catch(SerializationException)
			{
				return null;
			}
		}
		else
			return null;
	}
	public List<Student> Search3CourseInDorm()
	{

		var res = (from x in (deser[_extension](_filePath) as List<Student>)
				   where x.IsLivingInDorm == true && x.Course == "3"
				   select x).ToList();

		var New = GetAll();

		for(int i = 0; i < New.Count; i++)
		{
			foreach(var dormStudent in res)
			{
				if(New[i].Equals(dormStudent))
					New.RemoveAt(i);
			}
		} // Delete from file students with old rooms

		foreach(var dormStudent in res)
		{
			var residence = dormStudent.Residence.Split("-");
			if(dormStudent.Sex == "Male")
			{
				dormStudent.Residence = residence[0] + "-" + "1" + residence[1][1] + residence[1][2];
			}
			else if(dormStudent.Sex == "Female")
			{
				dormStudent.Residence = residence[0] + "-" + "2" + residence[1][1] + residence[1][2];
			}
			else
			{
				dormStudent.Residence = residence[0] + "-" + "3" + residence[1][1] + residence[1][2];
			}
			New.Add(dormStudent);
		}
		Clear();
		Add(New);
		return res;
	}

	#endregion

	#region Interact with Entities
	public Student CreateStudent(string firstName, string lastName, string sex,
		bool isLivingInDorm, string residence, string studentId, string course)
	{
		//Student student = new Student();
		Regex validName = new Regex(@"^[A-Z]+[a-z ]+$");
		Regex validId = new Regex(@"^[A-Z]{2}\s\d{8}$");
		Regex validDorm = new Regex(@"^\d{1,2}-\d{3}$");
		Regex validCourse = new Regex("[1-6]");

		string input = "";
		bool create = true;

		if(!validName.IsMatch(firstName))
		{
			input += "First Name, ";
			create = false;
		}

		if(!validName.IsMatch(lastName))
		{
			input += "Last Name, ";
			create = false;
		}

		if(!validName.IsMatch(sex))
		{
			input += "Sex, ";
			create = false;
		}

		if(!validId.IsMatch(studentId))
		{
			input += "Student Id, ";
			create = false;
		}

		if((isLivingInDorm && !validDorm.IsMatch(residence)) || (!isLivingInDorm && !validName.IsMatch(residence)))
		{
			input += "Residence, ";
			create = false;
		}

		if(!validCourse.IsMatch(course))
		{
			input += "Course, ";
			create = false;
		}

		if(create)
		{
			return new Student(firstName, lastName, sex, isLivingInDorm, residence, studentId, course);
		}
		else
			throw new MyException(input);
	}
	public Gardener CreateGardener(string firstName, string lastName, string sex,
		string residence, string employer)
	{
		Regex validName = new Regex(@"^[A-Z]+[a-z ]+$");

		string input = "";
		bool create = true;

		if(!validName.IsMatch(firstName))
		{
			input += "First Name, ";
			create = false;
		}

		if(!validName.IsMatch(lastName))
		{
			input += "Last Name, ";
			create = false;
		}

		if(!validName.IsMatch(sex))
		{
			input += "Sex, ";
			create = false;
		}

		if(!validName.IsMatch(residence))
		{
			input += "Residence, ";
			create = false;
		}

		if(!validName.IsMatch(employer))
		{
			input += "Employer, ";
			create = false;
		}

		if(create)
		{
			return new Gardener(firstName, lastName, sex, residence, employer);
		}
		else
			throw new MyException(input);

	}
	public Seller CreateSeller(string firstName, string lastName, string sex,
		 string residence, string product)
	{
		Regex validName = new Regex(@"^[A-Z]+[a-z ]+$");

		string input = "";
		bool create = true;

		if(!validName.IsMatch(firstName))
		{
			input += "First Name, ";
			create = false;
		}

		if(!validName.IsMatch(lastName))
		{
			input += "Last Name, ";
			create = false;
		}

		if(!validName.IsMatch(sex))
		{
			input += "Sex, ";
			create = false;
		}

		if(!validName.IsMatch(residence))
		{
			input += "Residence, ";
			create = false;
		}

		if(!validName.IsMatch(product))
		{
			input += "Product, ";
			create = false;
		}

		if(create)
		{
			return new Seller(firstName, lastName, sex, residence, product);
		}
		else
			throw new MyException(input);

	}
	#endregion
}