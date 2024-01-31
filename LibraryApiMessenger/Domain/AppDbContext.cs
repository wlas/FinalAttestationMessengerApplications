using Castle.Components.DictionaryAdapter.Xml;
using LibraryApiMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApiMessenger.Domain
{
	public partial class AppDbContext : DbContext
	{
		private readonly string _connectionString;
		public AppDbContext(string connectionString)
		{
			_connectionString = connectionString;
		}
		public AppDbContext() { }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseNpgsql(_connectionString).UseLazyLoadingProxies();
		//}

		public DbSet<Message> Messages { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }				

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Message>(e =>
			{
				e.HasKey(x => x.Id);
				e.HasIndex(x => x.SenderEmail).IsUnique();
				e.HasIndex(x => x.RecipientEmail).IsUnique();

				e.Property(e => e.TextMessage)
					.HasMaxLength(500);

				e.HasOne(x => x.Sender)
					.WithMany(x => x.SendMessages)
					.HasForeignKey(x => x.SenderEmail)
					.OnDelete(DeleteBehavior.Restrict);

				e.HasOne(x => x.Recipient)
					.WithMany(x => x.ReceiveMessages)
					.HasForeignKey(x => x.RecipientEmail)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<User>(e =>
			{
				e.HasKey(x => x.Id);
				e.HasIndex(x => x.Email).IsUnique();

				e.Property(e => e.Password)
					.HasMaxLength(30)
					.IsRequired();

				e.Property(e => e.Name)
					.HasMaxLength(50);

				e.HasOne(x => x.RoleType)
					.WithMany(x => x.Users);
			});			
		}
	}
}
