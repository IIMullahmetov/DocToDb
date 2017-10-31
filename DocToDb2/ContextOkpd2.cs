using System.Data.Entity;

namespace DocToDb2
{
	public class ContextOkpd2 : DbContext
	{
		public DbSet<Classifier> Classifiers { get; set; }

		public ContextOkpd2() : base("name=okpd2")
		{
		}
	}
}
