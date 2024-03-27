using Microsoft.EntityFrameworkCore;
using blogApp.Data;
using blogApp.Models;

namespace blogApp.Datacontext
{
	public class DataContext : DbContext
	{


		public DbSet<User> Users { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DataContext(DbContextOptions<DataContext> options)
			: base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Event>()
				.HasOne(e => e.User)
				.WithMany(u => u.Events)
				.HasForeignKey(e => e.UserId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Event>()
				.HasOne(e => e.Category)
				.WithMany(c => c.Events)
				.HasForeignKey(e => e.CategoryId)
				.IsRequired(false);


		}
	}
}
