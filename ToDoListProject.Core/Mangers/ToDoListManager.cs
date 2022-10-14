using AutoMapper;
using CSVWorker.Common.Extensions;
using CSVWorker.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Common.Extensions;
using ToDoListProject.Core.Mangers.Interfaces;
using ToDoListProject.DbModel.Models;
using ToDoListProject.ModelViews.ModelViews;
using ToDoListProject.ModelViews.ReadViews;
using ToDoListProject.ModelViews.Requset;

namespace ToDoListProject.Core.Mangers
{
    public class ToDoListManager : IToDoListManager
    {
        private TodoListDatabaseContext _dbContext;
        private IMapper _mapper;

        public ToDoListManager(TodoListDatabaseContext dbContext,IMapper mapper)
        {
            _dbContext= dbContext;
            _mapper= mapper;
        }
        
        public ToDoListModel CreateToDo(UserModel currentUser, ToDoRequset requset)
        {
            var checkAdmin = currentUser.IsAdmin;
            ToDo toDo= null;
            if (currentUser.Id == requset.UserId)
            {
                 toDo = _dbContext.ToDos.Add(new ToDo
                {
                    Title = requset.Title,
                    Contents = requset.Contents,
                    AssignedBy = currentUser.Id,
                    UserId = currentUser.Id,
                    Image=string.Empty
                    

                }).Entity;
            }
            else if(currentUser.Id != requset.UserId && !checkAdmin)
            {
                throw new ServiceValidationException("Not Allow to Create List for others");
            }else if (checkAdmin)
            {
                 toDo = _dbContext.ToDos.Add(new ToDo
                {
                    Title = requset.Title,
                    Contents = requset.Contents,
                    AssignedBy = currentUser.Id,
                    UserId = requset.UserId,
                    Image = string.Empty
                 }).Entity;

            }
            _dbContext.SaveChanges();
            return _mapper.Map<ToDoListModel>(toDo);           
        }

        public List<ToDoListModel> GetAllReadToDos()
        {
            _dbContext.IgnoreFilter = true;
            var res = _dbContext.ToDos.Where(a => a.IsRead).ToList();
              
            return _mapper.Map<List<ToDoListModel>>(res);
        }

        public ToDoResponse GetAllToDoLists(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            var queryRes = _dbContext.ToDos
                                      .Where(a => string.IsNullOrWhiteSpace(searchText)
                                                  || (a.Title.Contains(searchText)
                                                      || a.Contents.Contains(searchText)));

            if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderByDescending(sortColumn);
            }

            var res = queryRes.GetPaged(page, pageSize);

            var userIds = res.Data
                             .Select(a => a.Id)
                             .Distinct()
                             .ToList();

            var user = _dbContext.Users
                                     .Where(a => userIds.Contains(a.Id))
                                     .ToDictionary(a => a.Id, x => _mapper.Map<UserResult>(x));

            var data = new ToDoResponse()
            {
                ToDoList = _mapper.Map<PagedResult<ToDoListModel>>(res),
                User = user
            };

            data.ToDoList.Sortable.Add("Title", "Title");
            data.ToDoList.Sortable.Add("CreatedDate", "Created Date");

            return data;
        }



        public ToDoReadResponse ReadToDo(UserModel currentUser, ToDoReadRequast toDoReadRequast)
        {
            var user=_dbContext.ToDos.FirstOrDefault(a=> a.UserId == currentUser.Id);
            if(currentUser.Id == toDoReadRequast.Id)
            {
                user.IsRead = true;
                _dbContext.SaveChanges();
            }
            else
            {
                throw new ServiceValidationException("Cant Modifie it ..");
            }

            return _mapper.Map<ToDoReadResponse>(toDoReadRequast);
        }


        public ToDoListModel UpdateToDo(UserModel currentUser, ToDoUpdateModel request)
        {
            _dbContext.IgnoreFilter = true;
            var list = _dbContext.ToDos
                                    .FirstOrDefault(a => a.UserId == currentUser.Id &&currentUser.Id==request.UserId)
                                    ?? throw new ServiceValidationException("User not found");

            var url = "";

            if (!string.IsNullOrWhiteSpace(request.ImageString))
            {
                url = Helper.SaveImage(request.ImageString, "profileimages");
            }

            list.Title = request.Title;
            list.Contents = request.Contents;

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44389/";
                list.Image = @$"{baseURL}/api/v1/user/fileretrive/profilepic?filename={url}";
            }

            _dbContext.SaveChanges();
            return _mapper.Map<ToDoListModel>(list);
        }

        public void DeleteToDoList(UserModel currentUser, int id)
        {
            if (!currentUser.IsAdmin)
            {
                throw new ServiceValidationException("Cant Delete!");
            }
            var res = _dbContext.ToDos
                                   .FirstOrDefault(a => a.Id == id)
                                   ?? throw new ServiceValidationException("Invalid ToDo id received");
            res.IsArchived = true;
            _dbContext.SaveChanges();
        }
    }
}
