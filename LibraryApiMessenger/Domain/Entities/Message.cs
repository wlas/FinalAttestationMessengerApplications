namespace LibraryApiMessenger.Domain.Entities
{
	public class Message
	{
		public Guid Id { get; set; }
		public Guid SenderEmail { get; set; }
		public virtual User Sender { get; set; }
		public Guid RecipientEmail { get; set; }
		public virtual User Recipient { get; set; }
		public bool IsRead { get; set; }
		public DateTime CreatedDate { get; set; }
		public string TextMessage { get; set; }
	}
}
