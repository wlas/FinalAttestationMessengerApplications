using AutoMapper;
using LibraryApiMessenger.Domain.Entities;
using ServiceUserApi.Models;

namespace ServiceUserApi.Mapper
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<User, UserModel>().ConvertUsing(new EntityToModelConverter());
			CreateMap<User, Account>(MemberList.Destination);
			CreateMap<RegistrationModel, User>().ConvertUsing(new RegisterEntityConverter());
		}
	}
}
