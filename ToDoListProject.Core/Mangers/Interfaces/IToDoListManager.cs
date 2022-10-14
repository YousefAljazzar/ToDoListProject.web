using System.Collections.Generic;
using ToDoListProject.DbModel.Models;
using ToDoListProject.ModelViews.ModelViews;
using ToDoListProject.ModelViews.ReadViews;
using ToDoListProject.ModelViews.Requset;

namespace ToDoListProject.Core.Mangers.Interfaces
{
    public interface IToDoListManager :IManager
    {
        ToDoListModel CreateToDo(UserModel currentUser,ToDoRequset ToDoRequset);

        ToDoResponse GetAllToDoLists(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "");

        ToDoReadResponse ReadToDo(UserModel currentUser, ToDoReadRequast toDoReadRequast); 
      
        List<ToDoListModel> GetAllReadToDos();

        ToDoListModel UpdateToDo(UserModel currentUser, ToDoUpdateModel request);

        void DeleteToDoList(UserModel currentUser,int id);
    }
}
