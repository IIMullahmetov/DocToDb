using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace DocToDb
{
	public class Context : DbContext
	{
		public DbSet<Section> Sections { get; set; }
		public DbSet<Class> Classes { get; set; }
		public DbSet<Subclass> Subclasses { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<Subgroup> Subgroups { get; set; }
		public DbSet<Type> Types { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Subcategory> Subcategories { get; set; }

		public Context() : base("name=okpd")
		{
		}
	}
}
