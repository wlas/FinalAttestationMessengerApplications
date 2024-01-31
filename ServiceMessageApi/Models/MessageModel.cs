namespace ServiceMessageApi.Models
{
	public class MessageModel
	{
		public Guid Id { get; set; }
		public string SenderEmail { get; set; }
		public string RecipientEmail { get; set; }
		public bool IsRead { get; set; }
		public DateTime CreatedDate { get; set; }
		public string TextMessage { get; set; }
	}
}
