using AutoMapper;
using LibraryApiMessenger.Domain;
using LibraryApiMessenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceUserApi.Abstraction;
using ServiceUserApi.Models;
using System.Security.Cryptography;
using System.Text;

namespace ServiceUserApi.Services
{
	public class UserService : IUserService
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly Account _account;

		public UserService(IMapper mapper, AppDbContext context, Account account)
		{
			_mapper = mapper;
			_context = context;
			_account = account;
		}
		public Guid AddAdmin(RegistrationModel model)
		{
			using (_context)
			{
				var userResult = _context.Users.Count(x => x.RoleType.RoleUser == UserRole.Administrator);

				if (userResult > 0)
				{
					throw new Exception("Администратор может быть только один.");
				}

				var user = _mapper.Map<User>(model);

				user.RoleType = new Role
				{
					RoleUser = UserRole.Administrator
				};

				_context.Users.Add(user);
				_context.SaveChanges();

				return user.Id;
			}
		}

		public Guid AddUser(RegistrationModel model)
		{
			using (_context)
			{
				var userResult = _context.Users.FirstOrDefault(x => x.Email == model.Email.ToLower());

				if (userResult != null) 
				{
					throw new Exception("Пользователь не найден.");
				}

				var user = _mapper.Map<User>(model);
				user.RoleType = new Role { RoleUser = UserRole.User };

				_context.Users.Add(user);
				_context.SaveChanges();

				return user.Id;
			}
		}

		public UserModel Authentificate(LoginModel model)
		{
			User user = null;

			using (_context)
			{
				user = _context.Users.Include(x => x.RoleType).FirstOrDefault(x => x.Email == model.Email);
			}

			if (user == null)
			{
				throw new Exception("Пользователь не найден.");
			}

			if (CheckPassword(user.Salt, model.Password, user.Password))
			{
				return _mapper.Map<UserModel>(user);
				
			}
			else
			{
				throw new Exception("Неверный пароль.");
			}
		}

		public bool DeleteUser(Guid? userId, string? email)
		{
			if (_account.Role != UserRole.Administrator)
			{
				throw new Exception("Ограничение доступа.");
			}			

			using (_context)
			{
				var result = _context.Users.Include(x => x.RoleType).AsQueryable();
				if (!string.IsNullOrEmpty(email))
				{
					result = result.Where(x => x.Email == email);
				}
				if (userId.HasValue)
				{
					result = result.Where(x => x.Id == userId);
				}
				var userResult = result.FirstOrDefault();

				if (userResult == null) 
				{
					throw new Exception("Пользователь не найден.");
				}

				if (userResult.RoleType.RoleUser == UserRole.Administrator) 
				{
					throw new Exception("Ограничение доступа.");
				}

				_context.Users.Remove(userResult);
				_context.SaveChanges();
			}
			return true;
		}

		public UserModel GetUser(Guid? userId, string? email)
		{
			var user = new User();

			using (_context)
			{
				var result = _context.Users.Include(x => x.RoleType).AsQueryable();
				if (!string.IsNullOrEmpty(email))
					result = result.Where(x => x.Email == email);
				if (userId.HasValue)
					result = result.Where(x => x.Id == userId);

				user = result.FirstOrDefault();
			}

			if (user == null)
			{
				throw new Exception("Пользователь не найден.");
			}

			if (_account.Role == UserRole.Administrator || _account.Id == userId)
			{
				return _mapper.Map<UserModel>(user);
			}
            else
            {
				throw new Exception("Ограничение доступа.");
			}
        }

		public IEnumerable<UserModel> GetUsers()
		{
			var users = new List<UserModel>();

			if (_account.Role != UserRole.Administrator)
			{
				throw new Exception("Ограничение доступа.");
			}

			using (_context)
			{
				users.AddRange(_context.Users.Include(x => x.RoleType).Select(x => _mapper.Map<UserModel>(x)).ToList());
			}
			return users;
		}

		private bool CheckPassword(byte[] salt, string password, byte[] dbPassword)
		{
			var data = Encoding.ASCII.GetBytes(password).Concat(salt).ToArray();
			SHA512 shaM = new SHA512Managed();
			var pass = shaM.ComputeHash(data);

			return dbPassword.SequenceEqual(pass);
		}
		
	}
}
