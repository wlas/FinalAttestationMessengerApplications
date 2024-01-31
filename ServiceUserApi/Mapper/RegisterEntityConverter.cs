using AutoMapper;
using LibraryApiMessenger.Domain.Entities;
using ServiceUserApi.Models;
using System.Security.Cryptography;
using System.Text;

namespace ServiceUserApi.Mapper
{
	public class RegisterEntityConverter : ITypeConverter<RegistrationModel, User>
	{
		public User Convert(RegistrationModel source, User destination, ResolutionContext context)
		{

			var entity = new User
			{
				Id = Guid.NewGuid(),
				Email = source.Email.ToLower(),
				Name = source.Name,
				Surname = source.Surname,

			};

			entity.Salt = new byte[16];
			new Random().NextBytes(entity.Salt);
			var data = Encoding.ASCII.GetBytes(source.Password).Concat(entity.Salt).ToArray();
			SHA512 shaM = new SHA512Managed();
			entity.Password = shaM.ComputeHash(data);

			return entity;
		}
	}
}
