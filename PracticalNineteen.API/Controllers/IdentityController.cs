﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PracticalNineteen.Domain.DTO;
using PracticalNineteen.Domain.Entities;
using PracticalNineteen.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PracticalNineteen.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        readonly IAccountRepository _accountRepository;
        readonly IMapper _mapper;
        readonly IConfiguration _configuration;

        public IdentityController(IAccountRepository accountRepository, IMapper mapper, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Return list of users
        /// </summary>
        /// <returns></returns>
        [Route("Users")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<UserIdentity> users = await _accountRepository.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Assign user to role
        /// </summary>
        /// <param name="email"></param>
        /// <param name="role">name of role</param>
        /// <returns></returns>
        [Route("AddUserToRole")]
        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string email, string role)
        {
            var res = await _accountRepository.AddUserToRoleAsync(email, role);
            if (!res) return BadRequest(new { error = "User/Role does not exist" });

            return Ok(new { result = "User has been added to the role." });
        }

        /// <summary>
        /// Remove user from role
        /// </summary>
        /// <param name="email"></param>
        /// <param name="role">name of role</param>
        /// <returns></returns>
        [Route("RemoveUserToRole")]
        [HttpPost]
        public async Task<IActionResult> RemoveUserRole(string email, string role)
        {
            var res = await _accountRepository.RemoveUserFromRoleAsync(email, role);
            if (!res) return BadRequest(new { error = "User/Role does not exist" });

            return Ok(new { result = "User has been removed from the role." });
        }

        /// <summary>
        /// Returns all user roles
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Route("GetUserRoles")]
        [HttpPost]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            IEnumerable<string> roles = await _accountRepository.GetUserRolesAsync(email);
            if (email is null) return BadRequest(new { error = "User not exist!" });
            return Ok(roles);
        }

        /// <summary>
        /// handle new user registration request
        /// </summary>
        /// <param name="user">UserModel parameter</param>
        /// <returns></returns>
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel user)
        {
            if (!ModelState.IsValid) return BadRequest(new { error = "Please enter valid user details" });

            UserIdentity data = await _accountRepository.GetUserByEmailAsync(user.Email);
            if (data is not null) return BadRequest(new { error = "User already exist!" });

            UserIdentity userIdentity = _mapper.Map<UserIdentity>(user);
            bool isSucceeded = await _accountRepository.RegisterUserAsync(userIdentity);

            if (isSucceeded)
            {
                var token = GenerateJWT(userIdentity);
                return Ok(new { token = token });
            }
            return BadRequest(new { error = "User already exist!" });
        }

        /// <summary>
        /// Check given credentials and if it is valid than return token
        /// </summary>
        /// <param name="creds"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] CredentialModel creds)
        {
            if (!ModelState.IsValid) return BadRequest(new { error = "Please enter valid details!!" });

            var user = await _accountRepository.GetUserByEmailAsync(creds.Email);
            if (user is null) return BadRequest(new { error = "User not found!" });

            bool isCorrect = await _accountRepository.CheckUserCredsAsync(user, creds.Password);
            if (!isCorrect) return BadRequest(new { error = "Invalid credentials!" });

            string token = GenerateJWT(user);
            return Ok(new { token = token });
        }

        /// <summary>
        /// Return list of user roles
        /// </summary>
        /// <returns></returns>
        [Route("Roles")]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            IEnumerable<IdentityRole> roles = await _accountRepository.GetAllRolesAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Insert new role
        /// </summary>
        /// <param name="role">name of new role</param>
        /// <returns></returns>
        [Route("Roles")]
        [HttpPost]
        public async Task<IActionResult> CreateNewRole(string name)
        {
            if (name == null || name.Trim() == null) return BadRequest("Please enter role name!");
            bool isSucceeded = await _accountRepository.CreateRoleAsync(name);
            if (isSucceeded)
                return Ok(new { result = $"The role {name} has been added successfully" });
            return BadRequest(new { error = $"The role {name} has not been added" });
        }

        //[Route("Login")]
        //[HttpPost]
        //public async Task<IActionResult> LoginAsync(UserCredential user)
        //{
        //    if (!ModelState.IsValid) return BadRequest(user);
        //    var isValid = await _accountRepository.GetUserByUsernamePassword(user.UserName, user.Password);
        //    if (!isValid) return StatusCode(401, "Invalid username/password.");
        //    //var token = GenerateJWT();
        //    return Ok();
        //}

        /// <summary>
        /// create new JWT token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        private string GenerateJWT(UserIdentity user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new List<Claim>() {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT Token Id
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()) //time at which JWT issued
                }),
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwt = jwtTokenHandler.WriteToken(token);
            return jwt;
        }
    }
}
