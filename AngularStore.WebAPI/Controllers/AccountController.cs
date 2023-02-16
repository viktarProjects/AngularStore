using AngularStore.Core.Entities.Identity;
using AngularStore.Core.Interfaces;
using AngularStore.Core.Models;
using AngularStore.WebAPI.Dto_s;
using AngularStore.WebAPI.Errors;
using AngularStore.WebAPI.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AngularStore.WebAPI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IBasketService _basketService;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper, IBasketService basketService, IMessageService messageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _basketService = basketService;
            _messageService = messageService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);

            return CreateUserDto(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
            {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded || user == null) return Unauthorized(new ApiResponse(401));

            await UniteBaskets(user);

            return CreateUserDto(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "Email address has already been registered" }
                });
            }

            var user = new AppUser{
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded) await SendMessage(user);
            
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(new ApiResponse(200));
        }

        [HttpGet("ConfirmEmail", Name = "confirmation")]
        public async Task<ContentResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                var html = System.IO.File.ReadAllText(@"wwwroot/confirmed/confirmed.html");
                return base.Content(html, "text/html");
            }
            return null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserByClaimsPrincipalWithAddressAsync(HttpContext.User);

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindUserByClaimsPrincipalWithAddressAsync(HttpContext.User);

            user.Address = _mapper.Map<AddressDto, Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));

            return BadRequest("There are some problems during updating the address");
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        private void RemoveCookie(string key)
            {
            if (key == null) return;
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-1);
            option.Secure = true;
            option.IsEssential = true;
            Response.Cookies.Append(key, string.Empty, option);
            Response.Cookies.Delete(key);
        }

        private async Task SendMessage(AppUser user)
            {
            var toList = new List<string>() { user.Email };
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Link("confirmation", new { token, email = user.Email });
            var message = new MailData(toList, "Email confirmation", confirmationLink);
            _messageService.SendMessage(message);
            }

        private async Task UniteBaskets(AppUser user)
            {
            var anonBasketId = Request.Cookies["buyerId"];

            var userBasketId = user.UserName;

            await _basketService.UniteBasket(anonBasketId, userBasketId);

            RemoveCookie(Request.Cookies["buyerId"]);
        }

        private UserDto CreateUserDto(AppUser user)
        {
            return new UserDto
            {
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
