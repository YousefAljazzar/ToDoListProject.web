using AutoMapper;
using System;
using System.Linq;
using ToDoList.Common.Extensions;
using ToDoListProject.Core.Mangers.Interfaces;
using ToDoListProject.DbModel.Models;
using ToDoListProject.ModelViews.ModelViews;

namespace ToDoListProject.Core.Mangers
{
    public class CommonManger : ICommonManger
    {
        private TodoListDatabaseContext _todoListDatabaseContext;

        private IMapper _mapper;

      public CommonManger(TodoListDatabaseContext todoListDatabaseContext, IMapper mapper)
        {
            _todoListDatabaseContext = todoListDatabaseContext;
            _mapper = mapper;
        }

        public UserModel GetUserRole(UserModel user)
        {
            var dbUser = _todoListDatabaseContext.Users
                                                    .FirstOrDefault(a=> a.Id==user.Id)
                                                    ?? throw new ServiceValidationException("Invalid User id recevid");
            
            return _mapper.Map<UserModel>(dbUser);
          }
    }
}
