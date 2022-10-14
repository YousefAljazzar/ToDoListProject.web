using AutoMapper;
using CSVWorker.Common.Extensions;
using System.Collections.Generic;
using ToDoListProject.DbModel.Models;
using ToDoListProject.ModelViews.ModelViews;
using ToDoListProject.ModelViews.ReadViews;

namespace ToDoListProject.Core.Mapper
{
    public class Mapping :Profile
    {
        public Mapping()
        {
            CreateMap<User, LoginUserResponse>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<UserResult, User>().ReverseMap();
            CreateMap<PagedResult<UserModel>, PagedResult<User>>().ReverseMap();
            CreateMap<ToDoListModel, ToDo>().ReverseMap();
            CreateMap<ToDoListModel, List<ToDo>>().ReverseMap();
            CreateMap<PagedResult<ToDoListModel>, PagedResult<ToDo>>().ReverseMap();
            CreateMap<ToDoReadRequast, ToDoReadResponse>().ReverseMap();

        }
    }
}
