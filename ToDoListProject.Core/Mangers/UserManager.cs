using AutoMapper;
using CSVWorker.Common.Extensions;
using CSVWorker.Helper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ToDoList.Common.Extensions;
using ToDoListProject.Core.Mangers.Interfaces;
using ToDoListProject.DbModel.Models;
using ToDoListProject.ModelViews.ModelViews;

namespace ToDoListProject.Core.Mangers
{
    public class UserManager : IUserManager
    {
        private TodoListDatabaseContext _todoListDatabaseContext;

        private IMapper _mapper;

        public UserManager(TodoListDatabaseContext todoListDatabaseContext, IMapper mapper)
        {
            _todoListDatabaseContext = todoListDatabaseContext;
            _mapper = mapper;
        }

        #region public

        public LoginUserResponse SignUp(UserRegistrationModel userReg)
        {
            if (_todoListDatabaseContext.Users
                              .Any(a => a.Email.Equals(userReg.Email)))
            {
                throw new ServiceValidationException("User already exist");
            }
            if (userReg.Password != userReg.ConfirmPassword)
            {
                throw new ServiceValidationException("Password Dosent Match");
            }
            var hashedPassword=HashPassword(userReg.Password);

            var user = _todoListDatabaseContext.Users.Add(new User
            {
                FirstName=userReg.FirstName,
                LastName=userReg.LastName,
                Email=userReg.Email,
                Password=hashedPassword,          
                Image=string.Empty
            }).Entity;


            _todoListDatabaseContext.SaveChanges();

            var res = _mapper.Map<LoginUserResponse>(user);

            res.Token = $"Bearer {GenerateJWTToken(user)}";
            return res;

        }
        public LoginUserResponse Login(LoginUserRequset user)
        {

            var userdb = _todoListDatabaseContext.Users
                                   .FirstOrDefault(a => a.Email
                                                           .Equals(user.Email));

            if (userdb == null || !VerifyHashPassword(user.Password, userdb.Password))
            {
                throw new ServiceValidationException(300, "Invalid user name or password received");
            }

            var res = _mapper.Map<LoginUserResponse>(userdb);
            res.Token = $"Bearer {GenerateJWTToken(userdb)}";
            return res;
        }
     
        public void DeleteUser(UserModel currentUser, int id)
        {
            if (currentUser.Id == id)
            {
                throw new ServiceValidationException("You have no access to delete your self");
            }

            var user = _todoListDatabaseContext.Users
                                    .FirstOrDefault(a => a.Id == id)
                                    ?? throw new ServiceValidationException("User not found");
           
            user.IsArcived = true;
            _todoListDatabaseContext.SaveChanges();
        }

        public UserModel UpdateProfile(UserModel currentUser, UserModel request)
        {
            var user = _todoListDatabaseContext.Users
                                    .FirstOrDefault(a => a.Id == currentUser.Id)
                                    ?? throw new ServiceValidationException("User not found");

            var url = "";

            if (!string.IsNullOrWhiteSpace(request.ImageString))
            {
                url = Helper.SaveImage(request.ImageString, "profileimages");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44389/";
                user.Image = @$"{baseURL}/api/v1/user/fileretrive/profilepic?filename={url}";
            }

            _todoListDatabaseContext.SaveChanges();
            return _mapper.Map<UserModel>(user);
        }

        public UserModel GetUser(int id)
        {
            var user=_todoListDatabaseContext.Users
                                                .FirstOrDefault(a=> a.Id == id)
                                                ?? throw new ServiceValidationException("Invlaid User Id");

            return _mapper.Map<UserModel>(user);
        }


        public UserResponse GetAllUsers(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            var queryRes = _todoListDatabaseContext.Users
                                        .Where(a => string.IsNullOrWhiteSpace(searchText)
                                                    || (a.FirstName.Contains(searchText)
                                                        || a.LastName.Contains(searchText)));

            if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderByDescending(sortColumn);
            }

            var res = queryRes.GetPaged(page, pageSize);

            var todoIds = res.Data
                             .Select(a => a.Id)
                             .Distinct()
                             .ToList();

            var todos = _todoListDatabaseContext.ToDos
                                     .Where(a => todoIds.Contains(a.AssignedBy))
                                     .ToDictionary(a => a.Id, x => _mapper.Map<ToDoResult>(x));

            var data = new UserResponse()
            {
                User = _mapper.Map<PagedResult<UserModel>>(res),
                ToDoList = todos
            };

            data.User.Sortable.Add("Email", "Email");
            data.User.Sortable.Add("CreatedDate", "Created Date");

            return data;
        }



        #endregion public



        #region private 

        private static string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedPassword;
        }

        private static bool VerifyHashPassword(string password, string HashedPasword)
        {
            return BCrypt.Net.BCrypt.Verify(password, HashedPasword);
        }

        private string GenerateJWTToken(User user)
        {
            var jwtKey = "#test.key*&^vanthis%$^&*()$%^@#$@!@#%$#^%&*%^*";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("DateOfJoining", user.CreatedDate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var issuer = "test.com";

            var token = new JwtSecurityToken(
                        issuer,
                        issuer,
                        claims,
                        expires: DateTime.Now.AddDays(20),
                        signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion private  
    }
}
