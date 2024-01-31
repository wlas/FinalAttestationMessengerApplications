using AutoMapper;
using LibraryApiMessenger.Domain.Entities;
using ServiceMessageApi.Models;

namespace ServiceMessageApi.Mapper
{
	public class EntityToModelConverter : ITypeConverter<Message, MessageModel>
	{
		public MessageModel Convert(Message source, MessageModel destination, ResolutionContext context)
		{
			return new MessageModel
			{
				Id = source.Id,
				CreatedDate = source.CreatedDate,
				IsRead = source.IsRead,
				RecipientEmail = source.Recipient.Email,
				SenderEmail = source.Recipient.Email,
				TextMessage = source.TextMessage

			};
		}
	}
}
