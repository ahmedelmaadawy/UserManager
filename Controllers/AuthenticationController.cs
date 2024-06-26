﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User.Manager.API.Models;
using User.Manager.API.Models.Authentication.Login;
using User.Manager.API.Models.Authentication.SignUp;
using UserManagement.Service.Models;
using UserManagement.Service.Services;

namespace User.Manager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailService emailService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            if (registerUser == null || role == null)
                return BadRequest();

            //Checking if user exists in database
            var checkUser = await _userManager.FindByEmailAsync(registerUser.Email);
            if (checkUser != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Email already exists");
            }
            IdentityUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Username
            };
            //add user to db
            //admin - user 
            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    return StatusCode(201);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "This role doesnot exist");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            //Check if user exists in db
            var user = await _userManager.FindByNameAsync(loginModel.UserName);

            //Check if password is correct
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var userrole = await _userManager.GetRolesAsync(user);


                //claimlist creation
                var authenticationClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("uid",user.Id),
                    new Claim(ClaimTypes.Email,user.Email),
                    //global user id unique
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var userRoles = await _userManager.GetRolesAsync(user);
                //Add role to claims
                foreach (var role in userRoles)
                {
                    authenticationClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                //Generate the token with claims 
                var jwtToken = GetToken(authenticationClaims);

                //return the token
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    validfrom = jwtToken.ValidFrom,
                    expiration = jwtToken.ValidTo

                });
            }
            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> authenticationClaims)
        {
            var authenticationSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var Token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: authenticationClaims,
                    signingCredentials: new SigningCredentials(authenticationSigninKey, SecurityAlgorithms.HmacSha256)
                );
            return Token;
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotpasswordlink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Confirmation email link", forgotpasswordlink!);
                _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status200OK,

                        new Response { Status = "success", Message = $"PasswordChaged to email {user.Email}" });
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }


        [HttpGet("Resetpassword")]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };
            return Ok(new
            {
                model
            });

        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user != null)
            {
                var resetpasswordResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
                if (!resetpasswordResult.Succeeded)
                {
                    foreach (var error in resetpasswordResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }

                return StatusCode(StatusCodes.Status200OK,

                        new Response { Status = "success", Message = $"Password Changed for email {user.Email}" });
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Password didnt change server error");
        }
    }
}
