using AutoMapper;
using LibraryApiMessenger.Domain.Entities;
using ServiceMessageApi.Models;

namespace ServiceMessageApi.Mapper
{
	public class MessageProfile : Profile
	{
		public MessageProfile()
		{
			CreateMap<Message, MessageModel>().ConvertUsing(new EntityToModelConverter());
		}
	}
}
