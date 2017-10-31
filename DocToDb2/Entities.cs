using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocToDb2
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

	/// <summary>
	/// Таблица с левыми и правыми ключами
	/// </summary>
	[Table("Classifiers")]
	public class Classifier
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string Code { get; set; }

		public string Description { get; set; }

		public string LeftKey { get; set; }
	}
}
