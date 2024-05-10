using DocumentFormat.OpenXml.Spreadsheet;
using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Utilities.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly IUsersRepository _iUsersRepository;
        private readonly IRolesRepository _iRolesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IConfiguration config, IUsersRepository iUsersRepository,
            IRolesRepository iRolesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _iUsersRepository = iUsersRepository;
            _iRolesRepository = iRolesRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public string Login(LoginModel login)
        {
            var user = IsValidUser(login.UserName, login.PassWord);

            if (user == null)
            {
                return null; // hoặc ném một ngoại lệ hoặc trả về một kiểu dữ liệu cụ thể cho trường hợp xác thực không thành công
            }

            var token = GenerateJwtToken(user);
            return token;
        }
        private string GenerateJwtToken(UserToken userToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            { 
                new Claim(ClaimTypes.Name, userToken.UserName),
                new Claim(ClaimTypes.NameIdentifier, userToken.UserId.ToString()),
                new Claim("FullName", userToken.FullName),
                new Claim("Email", userToken.Email),
                new Claim("Image", userToken.Image),
                new Claim("Address", userToken.Address),
                new Claim(ClaimTypes.Role, userToken.RoleName)
            };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims: claims, // Thêm thông tin người dùng vào claims nếu cần
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpirationInMinutes"])), // Thời gian hết hạn của token
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserToken IsValidUser(string UserName, string PassWord)
        {
            // Kiểm tra thông tin đăng nhập từ nguồn dữ liệu (cơ sở dữ liệu, API, etc.)
            var password = XString.ToMD5(PassWord);
            var userToken = (from user in _iUsersRepository.GetList()
                        where user.UserName == UserName && user.Password == password
                        select new UserToken
                        {
                            UserId = user.UserId,
                            FullName = user.FullName,
                            UserName = user.UserName,
                            Address = user.Address,
                            Image = user.Image,
                            Email = user.Email,
                            RoleName = _iRolesRepository.GetById(user.RoleId).RoleName,
                        }).SingleOrDefault();
            return userToken;
        }

        public UserToken GetUserToken()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadJwtToken(token);

            return new UserToken
            {
                UserId = Convert.ToInt32(tokenS.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value),
                UserName = tokenS.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                FullName = tokenS.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value,
                Email = tokenS.Claims.FirstOrDefault(x => x.Type == "Email")?.Value,
                Address = tokenS.Claims.FirstOrDefault(x => x.Type == "Address")?.Value,
                Image = tokenS.Claims.FirstOrDefault(x => x.Type == "Image")?.Value,
                RoleName = tokenS.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
            };
        }
    }
}
