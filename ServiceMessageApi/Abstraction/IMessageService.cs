using ServiceMessageApi.Models;
using System.Collections.Generic;

namespace ServiceMessageApi.Abstraction
{
	public interface IMessageService
	{
		IEnumerable<MessageModel> GetMessages(string recipientEmail);
		IEnumerable<MessageModel> SendMessage(MessageModel model);
	}
}
