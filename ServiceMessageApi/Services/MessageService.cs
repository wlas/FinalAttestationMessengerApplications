using AutoMapper;
using LibraryApiMessenger.Domain;
using LibraryApiMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceMessageApi.Abstraction;
using ServiceMessageApi.Models;

namespace ServiceMessageApi.Services
{
	public class MessageService : IMessageService
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		public MessageService(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public IEnumerable<MessageModel> GetMessages(string recipientEmail)
		{
			var messageList = new List<MessageModel>();
			using (_context)
			{
				var messages = _context.Messages.Include(x => x.Recipient).Include(x => x.Sender).Where(x => x.Recipient.Email == recipientEmail && !x.IsRead).ToList();

				foreach (var message in messages)
				{
					message.IsRead = true;
				}

				_context.UpdateRange(messages);
				_context.SaveChanges();

				messageList.AddRange(messages.Select(_mapper.Map<MessageModel>));				
			}
			return messageList;
		}

		public IEnumerable<MessageModel> SendMessage(MessageModel model)
		{
			var messageList = new List<MessageModel>();
			using (_context)
			{
				var message = new Message()
				{
					SenderEmail = _context.Users.Single(x => x.Email == model.SenderEmail).Id,
					RecipientEmail = _context.Users.Single(x => x.Email == model.RecipientEmail).Id,
					CreatedDate = DateTime.UtcNow,
					IsRead = false,
					TextMessage = model.TextMessage,
				};
				_context.Messages.Add(message);
				_context.SaveChanges();

				messageList.Add(model);
			}
			return messageList;
		}
	}
}
