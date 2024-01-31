using LibraryApiMessenger.Domain.Entities;
using ServiceUserApi.Models;

namespace ServiceUserApi.Abstraction
{
	public interface IUserService
	{
		IEnumerable<UserModel> GetUsers();
		UserModel GetUser(Guid? userId, string? email);
		Guid AddUser(RegistrationModel model);
		Guid AddAdmin(RegistrationModel model);
		bool DeleteUser(Guid? userId, string? email);
		UserModel Authentificate(LoginModel model);
	}
}
