using DocumentFormat.OpenXml.Packaging;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace DocToDb
{
	public class Program
	{
		private static Section Section;
		private static Class Class;
		private static Subclass Subclass;
		private static Group Group;
		private static Subgroup Subgroup;
		private static Type Type;
		private static Category Category;
		private static Subcategory Subcategory;

		private static string Text { get; set; }

		private static Regex SubcategoryRegex;
		private static string SubcategoryPattern = "[0-9]{2,2}[.][0-9]{2,2}[.][0-9]{2,2}[.][0-9]{2,}";

		private static Regex CategoryRegex;
		private static string CategoryPattern = "[0-9]{2,2}[.][0-9]{2,2}[.][0-9]{2,2}[.][0-9]{1,}";

		private static Regex TypeRegex;
		private static string TypePattern = "[0-9]{2,2}[.][0-9]{2,2}[.][0-9]{2,}";

		private static Regex SubgroupRegex;
		private static string SubgroupPattern = "[0-9]{2,2}[.][0-9]{2,2}[.][0-9]{1,}";

		private static Regex GroupRegex;
		private static string GroupPattern = "[0-9]{2,2}[.][0-9]{2,}";

		private static Regex SubclassRegex;
		private static string SubclassPattern = "[0-9]{2,2}[.][0-9]{1,}";

		private static Regex ClassRegex;
		private static string ClassPattern = "[0-9]{2,2}";

		private static Regex SectionRegex = new Regex("РАЗДЕЛ [A-Z]{0,1}");

		private static Context context;

		public static void Main(string[] args)
		{
			context = new Context();
			SubclassRegex = new Regex(SubclassPattern);
			GroupRegex = new Regex(GroupPattern);
			SubgroupRegex = new Regex(SubgroupPattern);
			SubcategoryRegex = new Regex(SubcategoryPattern);
			ClassRegex = new Regex(ClassPattern);
			CategoryRegex = new Regex(CategoryPattern);
			TypeRegex = new Regex(TypePattern);
			
			Text = ReadAllTextFromDocx();			
			Console.Read();
		}

		private static string ReadAllTextFromDocx()
		{
			StringBuilder stringBuilder;
			using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(@"C:\okpd\okpd.docx", false))
			{
				NameTable nameTable = new NameTable();
				XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
				xmlNamespaceManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

				string wordprocessingDocumentText;
				using (StreamReader streamReader = new StreamReader(wordprocessingDocument.MainDocumentPart.GetStream()))
				{
					wordprocessingDocumentText = streamReader.ReadToEnd();
				}

				stringBuilder = new StringBuilder(wordprocessingDocumentText.Length);

				XmlDocument xmlDocument = new XmlDocument(nameTable);
				xmlDocument.LoadXml(wordprocessingDocumentText);

				XmlNodeList paragraphNodes = xmlDocument.SelectNodes("//w:p", xmlNamespaceManager);
				int i = 0;
				foreach (XmlNode paragraphNode in paragraphNodes)
				{
					XmlNodeList textNodes = paragraphNode.SelectNodes(".//w:t | .//w:tab | .//w:br", xmlNamespaceManager);
					foreach (XmlNode textNode in textNodes)
					{
						if (i > 0)
						{
							if (IsEntityType(textNode.InnerText))
							{
								Console.WriteLine(stringBuilder.ToString());
								AddEntity(ClearLine(stringBuilder.ToString()));
								stringBuilder.Clear();
								i = 0;
								context.BulkSaveChanges();
							}
						}
						switch (textNode.Name)
						{
							case "w:t":
								stringBuilder.Append(textNode.InnerText);
								break;

							case "w:tab":
								stringBuilder.Append("\t");
								break;

							case "w:br":
								stringBuilder.Append("\v");
								break;
						}
					}
					i++;
					stringBuilder.Append(Environment.NewLine);
				}
			}

			return stringBuilder.ToString();
		}

		private static string ClearLine(string line)
		{
			return Regex.Replace(line, @"\r\n", "");
		}

		private static void AddEntity(string line)
		{
			if (SubcategoryRegex.IsMatch(line))
			{
				Subcategory subcategory = new Subcategory()
				{
					Code = SubcategoryRegex.Match(line).Groups[0].Value,
					Description = SubcategoryRegex.Replace(line, "", 1),
					Category = Category
				};
				Subcategory = subcategory;
				context.Subcategories.Add(subcategory);
				return;
			}
			if (CategoryRegex.IsMatch(line))
			{
				Category category = new Category
				{
					Code = CategoryRegex.Match(line).Groups[0].Value,
					Description = CategoryRegex.Replace(line, "", 1),
					Type = Type
				};
				Category = category;
				context.Categories.Add(category);
				return;
			}
			if (TypeRegex.IsMatch(line))
			{
				Type type = new Type
				{
					Code = TypeRegex.Match(line).Groups[0].Value,
					Description = TypeRegex.Replace(line, "", 1),
					Subgroup = Subgroup
				};
				Type = type;
				context.Types.Add(type);
				return;
			}
			if (SubgroupRegex.IsMatch(line))
			{
				Subgroup subgroup = new Subgroup
				{
					Code = SubgroupRegex.Match(line).Groups[0].Value,
					Description = SubgroupRegex.Replace(line, "", 1),
					Group = Group
				};
				Subgroup = subgroup;
				context.Subgroups.Add(subgroup);
				return;
			}
			if (GroupRegex.IsMatch(line))
			{
				Group group = new Group
				{
					Code = GroupRegex.Match(line).Groups[0].Value,
					Description = GroupRegex.Replace(line, "", 1),
					Subclass = Subclass
				};
				Group = group;
				context.Groups.Add(group);
				return;
			}
			if (SubclassRegex.IsMatch(line))
			{
				Subclass subclass = new Subclass
				{
					Code = SubclassRegex.Match(line).Groups[0].Value,
					Description = SubclassRegex.Replace(line, "", 1),
					Class = Class
				};
				Subclass = subclass;
				context.Subclasses.Add(subclass);
				return;
			}
			if (ClassRegex.IsMatch(line))
			{
				Class @class = new Class
				{
					Code = ClassRegex.Match(line).Groups[0].Value,
					Description = ClassRegex.Replace(line, "", 1).Replace(@"\r", "").Replace(@"\n", "").Replace(@"\t", "").Replace(@"\v", ""),
					Section = Section
				};
				Class = @class;
				context.Classes.Add(@class);
				return;
			}
			if (SectionRegex.IsMatch(line))
			{
				Section section = new Section
				{
					Code = SectionRegex.Match(line).Groups[0].Value,
					Description = SectionRegex.Replace(line, "", 1)
				};
				Section = section;
				context.Sections.Add(section);
			}
		}

		private static bool IsEntityType(string line)
		{
			if (SubcategoryRegex.IsMatch(line))
			{
				return true;
			}
			if (CategoryRegex.IsMatch(line))
			{
				return true;
			}
			if (TypeRegex.IsMatch(line))
			{
				return true;
			}
			if (SubgroupRegex.IsMatch(line))
			{
				return true;
			}
			if (GroupRegex.IsMatch(line))
			{
				return true;
			}
			if (SubclassRegex.IsMatch(line))
			{ 
				return true;
			}
			if (ClassRegex.IsMatch(line))
			{
				return true;
			}
			if (SectionRegex.IsMatch(line))
			{
				return true;
			}
			return false;
		}
	}
}
