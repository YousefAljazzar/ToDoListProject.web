using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListProject.DbModel.Models;
using ToDoListProject.ModelViews.ModelViews;

namespace ToDoListProject.Core.Mangers.Interfaces
{
    public interface IUserManager : IManager
    {
         LoginUserResponse Login(LoginUserRequset user);

        LoginUserResponse SignUp(UserRegistrationModel user);

        UserModel GetUser(int id);

        UserModel UpdateProfile(UserModel currentUser, UserModel request);
        void DeleteUser(UserModel currentUser, int id);

        UserResponse GetAllUsers(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "");


    }
}
