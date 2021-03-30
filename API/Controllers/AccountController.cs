using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Interfaces;
using Core.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper, IMailService mailService)
        {
            _mapper = mapper;
            _mailService = mailService;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token=_tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email address is in use" } });
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            if (result.Succeeded)
            {
                await _mailService.SendEmailAsync(registerDto.Email, "Register Confirmed",
                    "<table border='0' cellpadding='0' cellspacing='0' align='center' width='600px' role='module' data-type='columns' style='padding:20px 10px 20px 20px;' bgcolor='black'>" +
                        "<tbody>" +
                            "<tr role='module-content'>" +
                                "<td height='100%' valign='top'>" +
                                    "<table class='column' width='540' style='width:540px; border-spacing:0; border-collapse:collapse; margin:0px 10px 0px 10px;' cellpadding='0' cellspacing='0' align='center' border='0' bgcolor=''> " +
                                        "<tbody> " +
                                            "<tr> " +
                                                "<td style='padding:0px;margin:0px;border-spacing:0;'>" +
                                                    "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='948e3f3f-5214-4721-a90e-625a47b1c957' data-mc-module-version='2019-10-22'> " +
                                                        "<tbody> " +
                                                            "<tr> " +
                                                                "<td style='padding:50px 30px 18px 30px; line-height:36px; text-align:inherit; background-color:#ffffff;' height='100%' valign='top' bgcolor='#ffffff' role='module-content'>" +
                                                                    "<div>" +
                                                                        "<div style='font-family: inherit; text-align: center'>" +
                                                                            "<img class='max-width' border='0' style='display:block; color:#000000; text-decoration:none; font-family:Helvetica, arial, sans-serif; font-size:16px;' width='100%' alt='' data-proportionally-constrained='true' data-responsive='false' src='https://www.wework.com/ideas/wp-content/uploads/sites/4/2019/08/Stocksy_txpbf16198csmR200_Medium_1873199_v1-1440x810-1-1120x630.jpg' height='189'>" +
                                                                            "<table class='module' role='module' data-type='spacer' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='27716fe9-ee64-4a64-94f9-a4f28bc172a0'>" +
                                                                                "<tbody>" +
                                                                                    "<tr>" +
                                                                                        "<td style='padding:0px 0px 30px 0px; background-color: white;' role='module-content' bgcolor=''></td>" +
                                                                                    "</tr>" +
                                                                                "</tbody>" +
                                                                            "</table>" +
                                                                            $"<span style='font-size: 43px'>Thanks for signing up, {user.DisplayName}! </span>" +
                                                                        "</div>" +
                                                                        "<div></div>" +
                                                                    "</div>" +
                                                                "</td> " +
                                                            "</tr> " +
                                                        "</tbody>" +
                                                    "</table>" +
                                                    "<table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='a10dcb57-ad22-4f4d-b765-1d427dfddb4e' data-mc-module-version='2019-10-22'> " +
                                                        "<tbody> " +
                                                            "<tr> " +
                                                                "<td style='padding:18px 30px 18px 30px; line-height:22px; text-align:inherit; background-color:#ffffff;' height='100%' valign='top' bgcolor='#ffffff' role='module-content'>" +
                                                                    "<div>" +
                                                                        "<div style='font-family: inherit; text-align: center'>" +
                                                                            "<span style='font-size: 18px;color: #005018;'>Please verify your email address to</span>" +
                                                                            "<span style='color: #000000; font-size: 18px; font-family: arial,helvetica,sans-serif'> get access to thousands of beautiful plants at</span>" +
                                                                            "<span style='font-size: 18px'>.</span>" +
                                                                           " <div class='align-text-center' style='font-size:20px;margin-top:30px;color: #005018;'><strong>Plantify Store</strong></div>" +
                                                                        "</div> " +
                                                                        "<div style='font-family: inherit; text-align: center'>" +
                                                                            "<span style='color: black; font-size: 18px'>" +
                                                                                "<strong>Thank you! </strong>" +
                                                                            "</span>" +
                                                                        "</div>" +
                                                                        "<div></div>" +
                                                                    "</div>" +
                                                                "</td> " +
                                                            "</tr> " +
                                                        "</tbody> " +
                                                    "</table>" +
                                                    "<table border='0' cellpadding='0' cellspacing='0' class='module' data-role='module-button' data-type='button' role='module' style='table-layout:fixed;' width='100%' data-muid='d050540f-4672-4f31-80d9-b395dc08abe1'> " +
                                                        "<tbody> " +
                                                            "<tr> " +
                                                                "<td align='center' bgcolor='#ffffff' class='outer-td' style='padding:0px 0px 0px 0px;'> " +
                                                                    "<table border='0' cellpadding='0' cellspacing='0' class='wrapper-mobile' style='text-align:center;'> " +
                                                                        "<tbody> " +
                                                                            "<tr> " +
                                                                                "<td align='center' bgcolor='#ffbe00' class='inner-td' style='border-radius:6px; font-size:16px; text-align:center; background-color:inherit;'> " +
                                                                                    "<a href='' style='background-color:black; border:1px solid black; border-color:#1cff00; border-radius:0px; border-width:1px; color:white; display:inline-block; font-size:14px; font-weight:normal; letter-spacing:0px; line-height:normal; padding:12px 40px 12px 40px; text-align:center; text-decoration:none; border-style:solid; font-family:inherit;' target='_blank'>Verify Email Now</a> " +
                                                                                "</td> " +
                                                                            "</tr> " +
                                                                        "</tbody> " +
                                                                    "</table> " +
                                                                "</td> " +
                                                            "</tr> " +
                                                        "</tbody> " +
                                                    "</table>" +
                                                    "<table class='module' role='module' data-type='spacer' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='27716fe9-ee64-4a64-94f9-a4f28bc172a0'>" +
                                                        "<tbody>" +
                                                            "<tr>" +
                                                                "<td style='padding:0px 0px 30px 0px; background-color: white;' role='module-content' bgcolor=''></td>" +
                                                            "</tr>" +
                                                        "</tbody>" +
                                                    "</table>" +
                                                "</td> " +
                                            "</tr> " +
                                        "</tbody> " +
                                    "</table>" +
                                "</td>" +
                            "</tr>" +
                        "</tbody>" +
                    "</table>"
                    //"<div style='background-color: white'>" +
                    //"<h1>Confirm</h1><p>Register at " + DateTime.Now + "</p>" +
                    //"</div>"
                    );
            }

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);

            return _mapper.Map<AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);

            user.Address = _mapper.Map<Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<AddressDto>(user.Address));

            return BadRequest("Problem updating the user");
        }
    }
}
