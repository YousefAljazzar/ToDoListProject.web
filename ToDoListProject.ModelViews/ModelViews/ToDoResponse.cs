using CSVWorker.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListProject.ModelViews.ModelViews
{
    public class ToDoResponse
    {
        public PagedResult<ToDoListModel> ToDoList { get; set; }

        public Dictionary<int, UserResult> User { get; set; }
    }
}
