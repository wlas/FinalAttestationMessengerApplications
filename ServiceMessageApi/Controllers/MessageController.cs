using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMessageApi.Abstraction;
using ServiceMessageApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ServiceMessageApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MessageController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IMessageService _messageService;
		public MessageController(IMapper mapper, IMessageService messageService)
		{
			_mapper = mapper;
			_messageService = messageService;
		}

		[Authorize]
		[HttpGet("GetMsg")]
		public IActionResult GetNewMessage()
		{
			var senderEmail = GetEmailFromToken().GetAwaiter().GetResult();

			var response = _messageService.GetMessages(senderEmail);
			return Ok(response);
		}
		[Authorize]
		[HttpPost("SendMsg")]
		public IActionResult SendMessage(string recipientEmail, string text)
		{			
			var senderEmail = GetEmailFromToken().GetAwaiter().GetResult();

			var message = new MessageModel
			{
				RecipientEmail = recipientEmail,
				SenderEmail = senderEmail,
				TextMessage = text
			};

			var response = _messageService.SendMessage(message);
			return Ok(response);
		}

		private async Task<string> GetEmailFromToken()
		{
			var token = await HttpContext.GetTokenAsync("access_token");

			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
			var claim = jwtToken!.Claims.Single(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

			return claim.Value;

		}
	}
}
