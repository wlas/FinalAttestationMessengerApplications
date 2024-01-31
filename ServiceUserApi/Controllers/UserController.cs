using LibraryApiMessenger.rsa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceUserApi.Abstraction;
using ServiceUserApi.Models;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;

namespace ServiceUserApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : Controller
	{
		private readonly IUserService _userService;
		private readonly Account _account;
		private readonly IConfiguration _configuration;


		public UserController(IUserService userService, Account account, IConfiguration configuration)
		{
			_account = account;
			_userService = userService;
			_configuration = configuration;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public IActionResult Login([Description("Аутентификация пользователя"), FromBody] LoginModel model)
		{
			if (!IsValidMailAdres(model.Email))
				return BadRequest($"Email:'{model.Email}' - Неверный формат почтового адреса");

			if (_account.GetAccessToken() != null)
				return BadRequest("Вы уже вошли в систему");
			var response = _userService.Authentificate(model);
			_account.Login(response);
			_account.RefreshToken(GenerateToken(_account));

			return Ok(_account.GetAccessToken());
		}

		[Authorize(Roles = "Administrator")]
		[HttpPost("addUser")]
		public IActionResult AddUser(RegistrationModel model)
		{
			if (!IsValidMailAdres(model.Email))
			{
				return BadRequest($"Email:'{model.Email}' - Неверный формат почтового адреса");
			}
			var response = _userService.AddUser(model);

			return Ok(response);
		}

		[AllowAnonymous]
		[HttpPost("addAdmin")]
		public IActionResult AddAdmin(RegistrationModel model)
		{
			if (!IsValidMailAdres(model.Email))
				return BadRequest($"Email:'{model.Email}' - Неверный формат почтового адреса");

			var response = _userService.AddAdmin(model);
			return Ok(response);
		}

		[Authorize(Roles = "Administrator")]
		[HttpGet("GetUsers")]
		public IActionResult GetUsers()
		{
			var response = _userService.GetUsers();
			return Ok(response);
		}

		[Authorize(Roles = "Administrator")]
		[HttpPost("GetUser")]
		public IActionResult GetUser(Guid? userId, string? email)
		{
			var response = _userService.GetUser(userId, email);
			return Ok(response);
		}

		[Authorize(Roles = "Administrator")]
		[HttpDelete]
		public IActionResult DeleteUser(Guid? userId, string? email)
		{
			var response = _userService.DeleteUser(userId, email);
			return Ok(response);
		}

		[HttpPost("logout")]
		public IActionResult LogOut()
		{
			_account.Logout();
			return Ok();
		}

		private string GenerateToken(Account model)
		{
			var key = new RsaSecurityKey(RsaEncryption.GetPrivateKey());
			var credential = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);
			var claims = new[]
			{
				new Claim(ClaimTypes.Email, model.Email),
				new Claim(ClaimTypes.Role, model.Role.ToString()),
				new Claim("UserId", model.Id.ToString())
			};
			var token = new JwtSecurityToken(
				_configuration["Jwt:Issuer"],
				_configuration["Jwt:Audience"],
				claims,
				expires: DateTime.Now.AddMinutes(60),
				signingCredentials: credential);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private bool IsValidMailAdres(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}
	}
}
