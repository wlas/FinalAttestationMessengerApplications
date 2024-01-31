namespace LibraryApiMessenger.Domain.Entities
{
	public class Role
	{
		public Guid Id { get; set; }
		public UserRole RoleUser { get; set; }
		public virtual List<User>? Users { get; set; }
	}
}
