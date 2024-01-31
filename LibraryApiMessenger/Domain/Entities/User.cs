namespace LibraryApiMessenger.Domain.Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public byte[] Password { get; set; }
		public byte[] Salt { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public virtual List<Message> SendMessages { get; set; }
		public virtual List<Message> ReceiveMessages { get; set; }
		public virtual Role RoleType { get; set; }
	}
}
