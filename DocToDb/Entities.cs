using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocToDb
{
	/// <summary>
	/// http://mydata.biz/ru/catalog/databases/okpd описание здесь
	/// </summary>
	public abstract class Base
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string Code { get; set; }

		public string Description { get; set; }
	}

	public class Section : Base
	{
		public Section() => Classes = new HashSet<Class>();

		public virtual ICollection<Class> Classes { get; set; }
	}

	/// <summary>
	/// Классы
	/// </summary>
	[Table("Classes")]
	public class Class : Base
	{
		public Class() => Subclasses = new HashSet<Subclass>();

		public virtual Section Section { get; set; }

		public virtual ICollection<Subclass> Subclasses { get; set; }
	}

	/// <summary>
	/// Подклассы
	/// </summary>
	[Table("Subclasses")]
	public class Subclass : Base
	{
		public Subclass() => Groups = new HashSet<Group>();

		public virtual Class Class { get; set; }

		public virtual ICollection<Group> Groups { get; set; }
	}

	/// <summary>
	/// Группа
	/// </summary>
	[Table("Groups")]
	public class Group : Base
	{
		public Group() => Subgroups = new HashSet<Subgroup>();

		public virtual Subclass Subclass { get; set; }

		public virtual ICollection<Subgroup> Subgroups { get; set; }
	}

	/// <summary>
	/// Подгруппа
	/// </summary>
	[Table("Subgroups")]
	public class Subgroup : Base
	{
		public Subgroup() => Types = new HashSet<Type>();

		public virtual Group Group { get; set; }

		public virtual ICollection<Type> Types { get; set; }
	}

	/// <summary>
	/// Вид
	/// </summary>
	[Table("Types")]
	public class Type : Base
	{
		public Type() => Categories = new HashSet<Category>();
		
		public virtual Subgroup Subgroup { get; set; }

		public virtual ICollection<Category> Categories { get; set; }
	}

	/// <summary>
	/// Категория
	/// </summary>
	[Table("Categories")]
	public class Category : Base
	{
		public Category() => Subcategories = new HashSet<Subcategory>();

		public virtual Type Type { get; set; }

		public virtual ICollection<Subcategory> Subcategories { get; set; }
	}

	/// <summary>
	/// Подкатегория
	/// </summary>
	[Table("Subcategories")]
	public class Subcategory : Base
	{
		public virtual Category Category { get; set; }
	}

	/// <summary>
	/// Таблица с левыми и правыми ключами
	/// </summary>
	[Table("Classifiers")]
	public class Classifier
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string Id { get; set; }

		public string Description { get; set; }

		public string LeftKey { get; set; }

		public string RightKey { get; set; }
	}

}
