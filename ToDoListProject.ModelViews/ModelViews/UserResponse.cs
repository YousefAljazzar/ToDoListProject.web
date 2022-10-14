using CSVWorker.Common.Extensions;
using System.Collections.Generic;

namespace ToDoListProject.ModelViews.ModelViews
{
    public class UserResponse
    {
        public PagedResult<UserModel> User { get; set; }

        public Dictionary<int, ToDoResult> ToDoList { get; set; }
    }
}
