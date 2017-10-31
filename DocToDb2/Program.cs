using Microsoft.Office.Interop.Word;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace DocToDb2
{
	class Program
	{
		private static string Code;
		private static string Description;
		private static Classifier Section;
		private static Classifier Class;
		private static Classifier Subclass;
		private static Classifier Group;
		private static Classifier Subgroup;
		private static Classifier Type;
		private static Classifier Category;
		private static Classifier Subcategory;

		private static ContextOkpd2 context;

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

		static void Main(string[] args)
		{
			context = new ContextOkpd2();
			SubclassRegex = new Regex(SubclassPattern);
			GroupRegex = new Regex(GroupPattern);
			SubgroupRegex = new Regex(SubgroupPattern);
			SubcategoryRegex = new Regex(SubcategoryPattern);
			ClassRegex = new Regex(ClassPattern);
			CategoryRegex = new Regex(CategoryPattern);
			TypeRegex = new Regex(TypePattern);
			Thread.CurrentThread.Priority = ThreadPriority.Highest;
			ParseDocTable();
		}

		private static void ParseDocTable()
		{
			Application app = new Application();
			Documents docs = app.Documents;
			Microsoft.Office.Interop.Word.Document doc = docs.Open(@"C:\okpd\okpd2.docx", ReadOnly: true);
			Microsoft.Office.Interop.Word.Table t = doc.Tables[1];
			Range r = t.Range;
			Cells cells = r.Cells;
			for (int i = 1; i <= cells.Count; i += 2)
			{
				Cell cell1 = cells[i];
				Range r1 = cell1.Range;
				Code = ClearLine(r1.Text);
				Cell cell2 = cells[i + 1];
				Range r2 = cell2.Range;
				Description = ClearLine(r2.Text);
				AddEntity();
				context.BulkSaveChanges();
				//Marshal.ReleaseComObject(cell);
				//Marshal.ReleaseComObject(r2);
			}

			doc.Close(false);
			app.Quit(false);
			Marshal.ReleaseComObject(cells);
			Marshal.ReleaseComObject(r);
			Marshal.ReleaseComObject(t);
			Marshal.ReleaseComObject(doc);
			Marshal.ReleaseComObject(docs);
			Marshal.ReleaseComObject(app);

		}

		private static string GetDescription(string s)
		{
			int index = s.IndexOf(Environment.NewLine) + 2;
			return ClearLine(s.Substring(index));
		}

		private static string GetCode(string s)
		{
			return s.Substring(0, s.IndexOf(Environment.NewLine));
		}

		private static string ClearLine(string line)
		{
			return Regex.Replace(Regex.Replace(line, @"\r\a", ""), @"\n", "");
		}

		private static void AddEntity()
		{
			if (SubcategoryRegex.IsMatch(Code))
			{
				Classifier subcategory = new Classifier()
				{
					Code = Code,
					Description = Description,
					LeftKey = Type.Code
				};
				Subcategory = subcategory;
				context.Classifiers.Add(subcategory);
				return;
			}
			if (CategoryRegex.IsMatch(Code))
			{
				Classifier category = new Classifier
				{
					Code = Code,
					Description = Description,
					LeftKey = Type.Code
				};
				Category = category;
				context.Classifiers.Add(category);
				return;
			}
			if (TypeRegex.IsMatch(Code))
			{
				Classifier type = new Classifier
				{
					Code = Code,
					Description = Description,
					LeftKey = Subgroup.Code
				};
				Type = type;
				context.Classifiers.Add(type);
				return;
			}
			if (SubgroupRegex.IsMatch(Code))
			{
				Classifier subgroup = new Classifier
				{
					Code = Code,
					Description = Description,
					LeftKey = Group.Code
				};
				Subgroup = subgroup;
				context.Classifiers.Add(subgroup);
				return;
			}
			if (GroupRegex.IsMatch(Code))
			{
				Classifier group = new Classifier
				{
					Code = Code,
					Description = Description,
					LeftKey = Subclass.Code
				};
				Group = group;
				context.Classifiers.Add(group);
				return;
			}
			if (SubclassRegex.IsMatch(Code))
			{
				Classifier subclass = new Classifier
				{
					Code = Code,
					Description = Description,
					LeftKey = Class.Code
				};
				Subclass = subclass;
				context.Classifiers.Add(subclass);
				return;
			}
			if (ClassRegex.IsMatch(Code))
			{
				Classifier @class = new Classifier
				{
					Code = Code,
					Description = Description,
					LeftKey = Section.Code
				};
				Class = @class;
				context.Classifiers.Add(@class);
				return;
			}
			if (SectionRegex.IsMatch(Code))
			{
				Classifier section = new Classifier
				{
					Code = Code,
					Description = Description
				};
				Section = section;
				context.Classifiers.Add(section);
			}
		}
	}
}
