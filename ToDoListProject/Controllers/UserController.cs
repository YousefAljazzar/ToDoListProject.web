using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using ToDoListProject.Core.Mangers.Interfaces;
using ToDoListProject.ModelViews.ModelViews;
using ToDoListWorker.Attributes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoListProject.Controllers
{

    [ApiController]
    
    public class UserController : ApiBaseController
    {

        private IUserManager _userManger;
        private readonly ILogger<UserController> _logger;

        
        public UserController(ILogger<UserController> logger,
                              IUserManager userManager)
        {
            _logger = logger;
            _userManger = userManager;
        }

        [Route("SignUp")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] UserRegistrationModel userReg)
        {
            var user = _userManger.SignUp(userReg);

            return Ok(user);


        }
        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginUserRequset userRequset)
        {
            var loging = _userManger.Login(userRequset);

            return Ok(loging);
        }

        
        [Route("GetUserById")]
        [ToDoListAuthrize()]
        [HttpGet]
        public IActionResult GetUser(int id)
        {
            var user=_userManger.GetUser(id);

            return Ok(user);
            
        }
        [Route("UpdateProfile")]
        [HttpPut]
        public IActionResult UpdateProfile( UserModel request)
        {
            var updated = _userManger.UpdateProfile(LoggedInUser,request);

            return Ok(updated);
        }

        [Route("Fileretrive/Profilepic")]
        [HttpGet]
        public IActionResult Retrive(string filename)
        {
            var folderPath = Directory.GetCurrentDirectory();
            folderPath = $@"{folderPath}\{filename}";
            var byteArray = System.IO.File.ReadAllBytes(folderPath);
            return File(byteArray, "image/jpeg", filename);
        }

        [Route("GetAllUsers")]
        [HttpGet]
        [ToDoListAuthrize()]
        public IActionResult GetAllUsers(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            var res=_userManger.GetAllUsers(page, pageSize, sortColumn, sortDirection, searchText);
            return Ok(res);
        }



        [Route("DeleteUser")]
        [HttpDelete]
        [ToDoListAuthrize()]
        public IActionResult DeleteUser(int id)
        {
            _userManger.DeleteUser(LoggedInUser, id);
            return Ok();

        }
    }
}
