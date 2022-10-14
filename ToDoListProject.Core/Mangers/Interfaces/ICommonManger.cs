using ToDoListProject.ModelViews.ModelViews;

namespace ToDoListProject.Core.Mangers.Interfaces
{
    public interface ICommonManger :IManager
    {
        public UserModel GetUserRole(UserModel user);

    }
}
