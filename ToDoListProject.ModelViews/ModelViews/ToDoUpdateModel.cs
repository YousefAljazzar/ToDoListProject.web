using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListProject.ModelViews.ModelViews
{
    public class ToDoUpdateModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public bool IsRead { get; set; }
        public string Image { get; set; }

        public string ImageString { get; set; }
        public int UserId { get; set; }
    }
}
