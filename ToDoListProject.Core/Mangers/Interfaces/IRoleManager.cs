using ToDoListProject.ModelViews.ModelViews;

namespace ToDoListProject.Core.Mangers.Interfaces
{
    public interface  IRoleManager :IManager
    {
        bool CheckAccess(UserModel userModel);

    }
}
