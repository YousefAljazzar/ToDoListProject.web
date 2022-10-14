using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListProject.Core.Mangers.Interfaces;
using ToDoListProject.DbModel.Models;
using ToDoListProject.ModelViews.ModelViews;

namespace ToDoListProject.Core.Mangers
{
    public class RoleManager :IRoleManager
    {
        private TodoListDatabaseContext _todoListDatabaseContext;
        private IMapper _mapper;

        public RoleManager(TodoListDatabaseContext todoListDatabaseContext, IMapper mapper)
        {
            _todoListDatabaseContext = todoListDatabaseContext;
            _mapper = mapper;
        }

        public bool CheckAccess(UserModel userModel)
        {
            var isAdmin = _todoListDatabaseContext.Users
                                       .Any(a => a.Id == userModel.Id
                                                 && a.IsAdmin);
            return isAdmin;
        }
    }
}
