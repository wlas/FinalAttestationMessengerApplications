using AutoMapper;
using LibraryApiMessenger.Domain.Entities;
using ServiceUserApi.Models;

namespace ServiceUserApi.Mapper
{
	public class EntityToModelConverter : ITypeConverter<User, UserModel>
	{
		public UserModel Convert(User source, UserModel destination, ResolutionContext context)
		{
			return new UserModel
			{
				Id = source.Id,
				Email = source.Email,
				Name = source.Name,
				Role = source.RoleType.RoleUser,
				Surname = source.Surname,

			};
		}
	}
}
